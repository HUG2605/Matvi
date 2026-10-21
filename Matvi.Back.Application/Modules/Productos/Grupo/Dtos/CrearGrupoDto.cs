using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Dtos
{
    public class CrearGrupoDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        //IdCatSeccion    smallyint	 1	no
        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtSecc_IdFld { get; set; }

        //Nombre varchar	50	no
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //Descripcion varchar	500	no
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DescripcionFld { get; set; }
    }
}
