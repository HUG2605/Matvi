using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
    public record ListarClienteSelectCoincidenciaQry(
        string coincidencia
        ) : IRequest<List<BaseDto<int>>>;
}
