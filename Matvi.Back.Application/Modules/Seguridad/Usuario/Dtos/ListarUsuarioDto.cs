using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
    public class ListarUsuarioDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string CtRol_Nombre { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaUltimoAcceso { get; set; }
        public bool Activo { get; set; }

    }
}
