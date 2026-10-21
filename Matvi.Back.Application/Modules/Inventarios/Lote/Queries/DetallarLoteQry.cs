using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Queries
{
    public record DetallarLoteQry(
        int id
        ) : IRequest<DetallarLoteDto>;
 
}
