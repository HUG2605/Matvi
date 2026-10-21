using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
    public record EditarClienteQry(
        int id
        ) : IRequest<EditarClienteDto>;
}
