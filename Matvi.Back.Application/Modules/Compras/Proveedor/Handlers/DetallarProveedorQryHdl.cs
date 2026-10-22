using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para detallar proveedor.
    /// </summary>
public class DetallarProveedorQryHdl(
      IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarProveedorQry, DetallarProveedorDto>
    {   
                /// <inheritdoc/>
public async Task<DetallarProveedorDto> Handle(
            DetallarProveedorQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProveedorEtt>()
                .Where(cp => cp.Id == request.id)
                .Select(cp => new DetallarProveedorDto
                {
                    Id = cp.Id,
                    Nombre = cp.Nombre,
                    Direccion = cp.Direccion,
                    Telefono = cp.Telefono,
                    NombreContacto = cp.NombreContacto,
                    TelefonoContacto = cp.TelefonoContacto,
                    Productos = cp.Coll_RlProductoProveedor_IdCtProveedor
                        .Select(pp => pp.Nav_IdCtProducto!.Nombre)
                        .ToList()
                });

            var proveedor = await query.SingleAsync(cancellationToken);
            return proveedor;
        }
    }
}
