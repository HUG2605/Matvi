using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar gasto.
    /// </summary>
public record DetallarGastoQry(int id)
    : IRequest<DetallarGastoDto>;

}
