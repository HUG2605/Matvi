using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
    public record ListarTipoGastoSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
