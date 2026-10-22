using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar pago.
    /// </summary>
public record DetallarPagoQry(int Id)
        : IRequest<DetallarPagoDto>;
}