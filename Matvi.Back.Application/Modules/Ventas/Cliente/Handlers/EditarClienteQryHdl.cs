using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    class EditarClienteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarClienteQry, EditarClienteDto>
    {
                /// <inheritdoc/>
public async Task<EditarClienteDto> Handle(
            EditarClienteQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtClienteEtt>()
                .Where(cc => cc.Id == request.id)
                .Select(cc => new EditarClienteDto {
                    Id = cc.Id,
                    NombreFld = cc.Nombre,
                    AliasFld = cc.Alias,
                    DireccionFld = cc.Direccion,
                    TelefonoFld = cc.Telefono,
                    IdCatRegimenFiscalFld = cc.IdCtRegimenFiscal,
                    RFCFld = cc.RFCFiscal,
                    CalleFiscalFld = cc.CalleFiscal,
                    NumeroExteriorFiscalFld = cc.NumeroExteriorFiscal,
                    NumeroInteriorFiscalFld = cc.NumeroInteriorFiscal,
                    CodigoPostalFiscalFld = cc.CodigoPostalFiscal,
                    ColoniaFiscalFld = cc.ColoniaFiscal,
                    LocalidadFiscalFld = cc.LocalidadFiscal,
                    MunicipioFiscalFld = cc.MunicipioFiscal,
                    EntidadFiscalFld = cc.EntidadFederativaFiscal,
                    LimiteCreditoSemanalFld = cc.LimiteCreditoSemanal

                });

            var cliente = await query.SingleAsync(cancellationToken);
            return cliente;
        }
    }
}
