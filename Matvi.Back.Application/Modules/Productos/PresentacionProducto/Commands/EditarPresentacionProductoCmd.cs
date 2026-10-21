using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands
{
    public record EditarPresentacionProductoCmd(
        EditarPresentacionProductoDto presentacionProducto
        ) : IRequest<OperationResult<bool>>
    {
    
    }
}
