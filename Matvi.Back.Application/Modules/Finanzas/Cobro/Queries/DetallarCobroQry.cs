using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar cobro.
    /// </summary>
public record DetallarCobroQry(int Id)
        : IRequest<DetallarCobroDto>;
}