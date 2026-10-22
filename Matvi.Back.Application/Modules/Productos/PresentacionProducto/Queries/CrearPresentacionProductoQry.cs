using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de crear presentacion producto.
    /// </summary>
public record CrearPresentacionProductoQry(
        int productoId
        ) : IRequest<CrearPresentacionProductoDto>;
}
