using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para cerrar corte caja.
    /// </summary>
public class CerrarCorteCajaDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

                /// <summary>
        /// Identificador único de ct sucu id.
        /// </summary>
public byte CtSucu_Id { get; set; }
                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
public short CtUsua_Id { get; set; }

                /// <summary>
        /// Propiedad que representa fondo final contado.
        /// </summary>
[Display(Name = "Fondo final contado")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a cero.")]
        public decimal FondoFinalContado { get; set; }

                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
[Display(Name = "Notas de cierre")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }
    }
}