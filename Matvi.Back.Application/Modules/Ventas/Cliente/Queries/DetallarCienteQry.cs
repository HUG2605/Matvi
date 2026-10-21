using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
    public record DetallarCienteQry(
        int id
        ) : IRequest<DetallarClienteDto>;
}
