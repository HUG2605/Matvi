using System.Collections.Generic;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CuentaBancaria.Queries
{
    public record ListarCuentaBancariaSelectQry : IRequest<List<BaseDto<byte>>>;
}
