using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar tipo gasto select.
    /// </summary>
public record ListarTipoGastoSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
