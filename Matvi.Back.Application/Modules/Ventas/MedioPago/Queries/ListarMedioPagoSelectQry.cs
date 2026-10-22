using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.MedioPago.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar medio pago select.
    /// </summary>
public record ListarMedioPagoSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
