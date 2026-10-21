using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries
{
    public record DetallarMovimientoEfectivoQry(
        string TipoOrigen,
        int Id
    ) : IRequest<DetallarMovimientoEfectivoDto?>;
}
