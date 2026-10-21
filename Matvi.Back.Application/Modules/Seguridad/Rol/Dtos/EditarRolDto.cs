using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
    public class EditarRolDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        [Display(Name = "Nombre")]
        public string NombreFld { get; set; } = string.Empty;

        public List<RolFuncionDto>? Funciones { get; set; }

    }
}
