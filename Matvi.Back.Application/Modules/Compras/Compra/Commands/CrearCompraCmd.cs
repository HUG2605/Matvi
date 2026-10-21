using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Commands
{
    public record CrearCompraCmd(
        CrearCompraDto compra
        ):IRequest<OperationResult<bool>>;

}
 