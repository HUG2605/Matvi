using System.Collections.Generic;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CuentaBancaria.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar cuenta bancaria select.
    /// </summary>
public record ListarCuentaBancariaSelectQry : IRequest<List<BaseDto<byte>>>;
}
