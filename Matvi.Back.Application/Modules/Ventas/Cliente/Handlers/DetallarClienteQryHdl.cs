using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    class DetallarClienteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarCienteQry, DetallarClienteDto>
    {
                /// <inheritdoc/>
public async Task<DetallarClienteDto> Handle(
            DetallarCienteQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtClienteEtt>()
                        .Where(cc => cc.Id == request.id)
                        .Select(cc => new DetallarClienteDto
                        {
                            Id = cc.Id,
                            Nombre = cc.Nombre,
                            Alias = cc.Alias,
                            Direccion = cc.Direccion,
                            Telefono = cc.Telefono,
                            CtRegiFisc_Nombre = cc.Nav_IdCtRegimenFiscal!.Nombre,
                            RFCFiscal = cc.RFCFiscal,
                            CalleFiscal = cc.CalleFiscal,
                            NumeroExteriorFiscal = cc.NumeroExteriorFiscal,
                            NumeroInteriorFiscal = cc.NumeroInteriorFiscal,
                            CodigoPostalFiscal = cc.CodigoPostalFiscal,
                            ColoniaFiscal = cc.ColoniaFiscal,
                            LocalidadFiscal = cc.LocalidadFiscal,
                            MunicipioFiscal = cc.MunicipioFiscal,
                            EntidadFederativaFiscal = cc.EntidadFederativaFiscal,
                            LimiteCreditoSemanal = cc.LimiteCreditoSemanal
                        });
            
            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}