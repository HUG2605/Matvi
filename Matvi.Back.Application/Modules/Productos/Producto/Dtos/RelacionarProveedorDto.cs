using Matvi.Back.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
    public class RelacionarProveedorDto
    {
        public int CtProv_Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public bool EstaRelacionado { get; set; }
    }
}
