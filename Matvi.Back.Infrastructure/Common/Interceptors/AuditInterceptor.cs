using Matvi.Back.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Matvi.Back.Infrastructure.Common.Interceptors
{
    /// <summary>
    /// Interceptor de Entity Framework Core para registrar en bitácora (<see cref="BtCambioEtt"/>) los cambios (inserciones, modificaciones y eliminaciones) de entidades auditables.
    /// </summary>
    public class AuditInterceptor : SaveChangesInterceptor
    {
        /// <summary>
        /// Identificador del usuario que realiza la operación actual, asignado antes de persistir cambios.
        /// </summary>
        public short UsuarioId { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AuditInterceptor"/>.
        /// </summary>
        public AuditInterceptor()
        {
        }

        /// <summary>
        /// Intercepta la operación de guardado de cambios de forma asíncrona para registrar en bitácora las modificaciones realizadas sobre las entidades auditables.
        /// </summary>
        /// <param name="eventData">Datos del evento de contexto que contiene el <see cref="Microsoft.EntityFrameworkCore.DbContext"/> y la información de la operación.</param>
        /// <param name="result">Resultado de la interception que puede suprimir o modificar la operación original.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
        /// <returns>El resultado de la interception con el número de entidades afectadas.</returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            short usuarioId = UsuarioId; // ← YA NO SE RESUELVE DESDE DI

            var bitacoras = new List<BtCambioEtt>();

            foreach (var entry in context.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable
                            && e.Entity.GetType() != typeof(BtCambioEtt)
                            && (e.State == EntityState.Added
                                || e.State == EntityState.Modified
                                || e.State == EntityState.Deleted)))
            {
                var tabla = entry.Metadata.GetTableName();
                var pk = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "N/A";

                if (entry.State == EntityState.Added)
                {
                    foreach (var prop in entry.Properties)
                    {
                        bitacoras.Add(new BtCambioEtt
                        {
                            Tabla = tabla!,
                            LlavePrimaria = pk,
                            Campo = prop.Metadata.Name,
                            ValorAnterior = "[NUEVO]",
                            ValorNuevo = prop.CurrentValue?.ToString() ?? "",
                            TipoOperacion = (byte)OperationType.INSERT,
                            IdCtUsuario = usuarioId,
                            Fecha = DateTime.Now
                        });
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified && (prop.OriginalValue?.ToString() != prop.CurrentValue?.ToString()))
                        {
                            bitacoras.Add(new BtCambioEtt
                            {
                                Tabla = tabla!,
                                LlavePrimaria = pk,
                                Campo = prop.Metadata.Name,
                                ValorAnterior = prop.OriginalValue?.ToString() ?? "",
                                ValorNuevo = prop.CurrentValue?.ToString() ?? "",
                                TipoOperacion = (byte)OperationType.UPDATE,
                                IdCtUsuario = usuarioId,
                                Fecha = DateTime.Now
                            });
                        }
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    foreach (var prop in entry.Properties)
                    {
                        bitacoras.Add(new BtCambioEtt
                        {
                            Tabla = tabla!,
                            LlavePrimaria = pk,
                            Campo = prop.Metadata.Name,
                            ValorAnterior = prop.OriginalValue?.ToString() ?? "",
                            ValorNuevo = "[ELIMINADO]",
                            TipoOperacion = (byte)OperationType.DELETE,
                            IdCtUsuario = usuarioId,
                            Fecha = DateTime.Now
                        });
                    }
                }
            }

            if (bitacoras.Any())
            {
                context.AddRange(bitacoras);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
