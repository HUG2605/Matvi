using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para detallar lote.
    /// </summary>
public class DetallarLoteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarLoteQry, DetallarLoteDto>
    {
                /// <inheritdoc/>
public async Task<DetallarLoteDto> Handle(
            DetallarLoteQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpLoteEtt>()
                .Where(x => x.Id == request.id)
                .Select(x => new DetallarLoteDto
                {
                    //IIdDto
                    Id = x.Id,
                    //IFolioDto
                    CtSucu_Clave = x.Nav_IdCtSucursal.Clave,
                    Numero = x.Numero,
                    //IProductoBaseDto
                    CtProd_Nombre = x.Nav_Cpp_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = x.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = x.Nav_Cpp_IdCtProducto.Marca,
                    // iPresentacionProductoBaseDto
                    CtPresProd_IdCtProducto = x.Cpp_IdCtProducto,
                    CtPresProd_Nombre = x.Cpp_Nombre,
                    CtPresProd_FactorUBI = x.Cpp_FactorUBI,
                    // Otros
                    CtSucu_Nombre = x.Nav_IdCtSucursal.Nombre,
                    CtTipoLote_Id = x.Nav_IdCtTipoLote.Id,
                    CtTipoLote_Nombre = x.Nav_IdCtTipoLote.Nombre,
                    CantidadInicialUBI = x.CantidadInicialUBI,
                    CantidadDisponibleUBI = x.CantidadDisponibleUBI,
                    CostoUnitario = x.CostoUP,
                    CostoUnitarioBase = x.CostoUBI,
                    Fecha = x.Fecha,
                    CtUnidBaseInve_Abreviatura = x.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura
                });

            var lote = await query.SingleAsync(cancellationToken);
            return lote;
        }
    }
}
