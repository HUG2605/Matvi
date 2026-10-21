using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
    public record CrearPresentacionProductoQry(
        int productoId
        ) : IRequest<CrearPresentacionProductoDto>;
}
