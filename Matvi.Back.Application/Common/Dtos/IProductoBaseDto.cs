using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
    public interface IProductoBaseDto
    {
        string CtProd_Nombre { get; set; }
        string? CtProd_EspecificacionTecnica { get; set; }
        string? CtProd_Marca { get; set; }
        string CtProd_NombreCompleto { get; }
    }
}
