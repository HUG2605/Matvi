using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Queries
{
    public record DetallarCobroQry(int Id)
        : IRequest<DetallarCobroDto>;
}