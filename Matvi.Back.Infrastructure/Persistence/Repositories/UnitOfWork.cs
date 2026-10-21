using Matvi.Back.Infrastructure.Common.Interceptors;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Transversal.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using System.Text.RegularExpressions;

namespace Matvi.Back.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(
        MatviCmdDbCtx context,
        AuditInterceptor audit,
        Func<short> usuarioProvider
        ) : IUnitOfWork
    {
        private readonly MatviCmdDbCtx _context = context;
        private readonly AuditInterceptor _audit = audit;
        private readonly Func<short> _usuarioProvider = usuarioProvider;
        private IDbContextTransaction? _transaction;

        #region Transacciones

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            }
            return _transaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        #endregion

        #region Persistencia

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                _audit.UsuarioId = _usuarioProvider(); // ← AQUÍ SE ASIGNA EL USUARIO
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                ManejarExcepcionTransaccional(ex);
                throw; // si no fue manejada, relanza
            }
        }

        #endregion

        #region CRUD genérico

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }
            entry.State = EntityState.Deleted;
        }
      
        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                var entry = _context.Entry(entity);
                // Si la entidad no está siendo rastreada, la adjuntamos
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }
                // Marcamos como eliminada (esto dispara tu auditoría automática)
                entry.State = EntityState.Deleted;
            }
        }

        public async Task<TEntity?> FindAsync<TEntity, TId>(TId id, CancellationToken cancellationToken) where TEntity : class
        {
            return await _context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        #endregion

        #region Métodos para adjuntar padres automáticamente

        public void AttachParentsRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
                AttachParents(entity);
        }

        void AttachParents<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);

            foreach (var fk in entry.Metadata.GetForeignKeys())
            {
                var principalType = fk.PrincipalEntityType.ClrType;

                // Obtener valores FK
                var fkValues = fk.Properties
                    .Select(p => entry.Property(p.Name).CurrentValue)
                    .ToArray();

                // Crear instancia padre
                var principal = Activator.CreateInstance(principalType);

                // Asignar PKs
                for (int i = 0; i < fk.PrincipalKey.Properties.Count; i++)
                {
                    var pkProp = fk.PrincipalKey.Properties[i].Name;
                    principalType.GetProperty(pkProp)?.SetValue(principal, fkValues[i]);
                }

                // Buscar si ya está trackeado
                var tracked = _context.ChangeTracker.Entries()
                    .FirstOrDefault(e =>
                        e.Entity.GetType() == principalType &&
                        fk.PrincipalKey.Properties.All(pk =>
                        {
                            var pkName = pk.Name;
                            var pkValue = principalType.GetProperty(pkName)?.GetValue(principal);
                            var trackedValue = e.Property(pkName).CurrentValue;
                            return Equals(pkValue, trackedValue);
                        }));

                if (tracked != null)
                {
                    // Asignar navegación padre
                    var navName = fk.DependentToPrincipal?.Name;
                    if (navName != null)
                        entry.Reference(navName).CurrentValue = tracked.Entity;

                    continue;
                }

                // 🔥 MARCAR COMO EXISTENTE (clave)
                _context.Entry(principal).State = EntityState.Unchanged;

                // Asignar navegación padre
                var nav = fk.DependentToPrincipal?.Name;
                if (nav != null)
                    entry.Reference(nav).CurrentValue = principal;
            }
        }

        #endregion

        #region Privates

        private void ManejarExcepcionTransaccional(Exception ex)
        {
            var mensaje = ObtenerMensajeCompleto(ex);

            // Log diagnóstico: ver inner exception real en la consola de salida.
            // Esto ayuda a identificar la causa raíz cuando el handler revienta
            // con un DbUpdateException genérico.
            System.Diagnostics.Debug.WriteLine("=== UnitOfWork.SaveChangesAsync EXCEPCIÓN ===");
            System.Diagnostics.Debug.WriteLine(mensaje);
            System.Diagnostics.Debug.WriteLine("=== STACK ===");
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            System.Diagnostics.Debug.WriteLine("=============================================");

            if (EsViolacionDeFK(mensaje))
                throw new InformativeException("La operación no se puede realizar: el registro tiene vínculos con información existente.");

            if (EsViolacionDeUK(mensaje))
            {
                var clave = ExtraerClaveDuplicada(mensaje);
                throw new DuplicateKeyException($"El valor que intenta guardar ya existe y no puede repetirse. Valor duplicado: {clave}.");
            }

            // Si no es FK ni UK, lanzar el mensaje real del inner exception
            // envuelto en InvalidOperationException para que el caller (o el
            // handler que atrape) vea la causa raíz en vez de un
            // DbUpdateException genérico.
            var inner = ex.InnerException?.Message ?? ex.Message;
            throw new InvalidOperationException(
                $"No se pudo guardar el cambio. Detalle SQL: {inner}", ex);
        }

        private static bool EsViolacionDeFK(string mensaje) =>
            mensaje.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase)
            || mensaje.Contains("conflicted with the REFERENCE constraint", StringComparison.OrdinalIgnoreCase)
            || mensaje.Contains("violates the foreign key constraint", StringComparison.OrdinalIgnoreCase);

        private static bool EsViolacionDeUK(string mensaje) =>
            mensaje.Contains("The duplicate key value is", StringComparison.OrdinalIgnoreCase)
            || mensaje.Contains("Violation of UNIQUE KEY constraint", StringComparison.OrdinalIgnoreCase);

        private static string ExtraerClaveDuplicada(string mensaje)
        {
            var match = Regex.Match(mensaje, @"The duplicate key value is \((.*?)\)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : "valor desconocido";
        }

        private static string ObtenerMensajeCompleto(Exception ex)
        {
            var sb = new StringBuilder();
            for (var cur = ex; cur is not null; cur = cur.InnerException)
                sb.AppendLine(cur.Message);
            return sb.ToString();
        }

        #endregion

    }
}
