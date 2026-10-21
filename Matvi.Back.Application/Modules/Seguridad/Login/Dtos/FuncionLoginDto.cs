using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
    public class FuncionLoginDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
        public string? Ruta { get; set; }
        public bool RequiereSucursal { get; set; }
        public byte? CtModu_Id { get; set; }
        public string? CtModu_Nombre { get; set; }
        public string? CtModu_Area { get; set; }
        public string? CtModu_IconName { get; set; }
        public bool VerSensibles { get; set; }
    }
}
