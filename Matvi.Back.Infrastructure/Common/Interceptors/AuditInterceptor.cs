using Matvi.Back.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Matvi.Back.Infrastructure.Common.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        // El UnitOfWork asigna este valor antes de SaveChanges
        public short UsuarioId { get; set; }

        public AuditInterceptor()
        {
        }

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
