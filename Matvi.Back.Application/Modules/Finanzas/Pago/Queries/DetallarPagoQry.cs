using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Queries
{
    public record DetallarPagoQry(int Id)
        : IRequest<DetallarPagoDto>;
}