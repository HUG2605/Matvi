using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
    public class SucursalLoginDto
    {
        public byte? Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}