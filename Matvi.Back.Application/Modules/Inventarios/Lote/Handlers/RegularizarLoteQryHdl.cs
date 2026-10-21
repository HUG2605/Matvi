using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Handlers
{
    class RegularizarLoteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<RegularizarLoteQry, RegularizarLoteDto>
    {
        public async Task<RegularizarLoteDto> Handle(
            RegularizarLoteQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = from l in db.Set<OpLoteEtt>()
                        where l.Id == request.id
                        select new RegularizarLoteDto
                        {
                            // IIdDto
                            Id = l.Id,
                            // IFolioDto
                            CtSucu_Clave = l.Nav_IdCtSucursal.Clave,
                            Numero = l.Numero,
                            // IProductoBaseDto
                            CtProd_Nombre = l.Nav_Cpp_IdCtProducto.Nombre,
                            CtProd_EspecificacionTecnica = l.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                            CtProd_Marca = l.Nav_Cpp_IdCtProducto.Marca,
                            // IPresentacionProductoBaseDto
                            CtPresProd_Nombre = l.Cpp_Nombre,
                            CtPresProd_FactorUBI = l.Cpp_FactorUBI,
                            // Otros
                            CtSucu_Nombre = l.Nav_IdCtSucursal.Nombre,
                            CtTipoLote_Nombre = l.Nav_IdCtTipoLote.Nombre,
                            CostoUnitarioFld = l.CostoUP,
                            Fecha = l.Fecha,
                            CtUnidBaseInve_Abreviatura =l.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura
                        };

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
