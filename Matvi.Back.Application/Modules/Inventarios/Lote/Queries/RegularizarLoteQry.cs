using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Queries
{
    public record RegularizarLoteQry(
        int id
        ) : IRequest<RegularizarLoteDto>;
}
