using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries
{
    public record ListarGastosDeCajaQry(
        int IdOpCorteCaja,
        FilterGrd Filter
    ) : IRequest<DataGrd<ListarGastoCajaDto>>;
}
