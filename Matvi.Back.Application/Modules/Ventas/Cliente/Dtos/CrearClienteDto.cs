
using Matvi.Back.Application.Common.Constants;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear cliente.
    /// </summary>
public class CrearClienteDto : IIdDto<int>
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

        //DATOS GEBNERALES DEL CLIENTES 

        //Nombre varchar	50	no
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //Direccion varchar	100	no
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
[Display(Name = "Dirección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DireccionFld { get; set; }

        //Telefono	varchar	10	no
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
[Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string TelefonoFld { get; set; }

        //Alias varchar 25 yes
                /// <summary>
        /// Propiedad que representa alias.
        /// </summary>
[Display(Name = "Alias")]
        [StringLength(25, ErrorMessage = "El valor para {0} no debe exceder {1} caracteres.")]
        public string? AliasFld { get; set; }

        //DATOS FISCALES DEL CLIENTE 

        //RFC varchar 13 no
                /// <summary>
        /// Propiedad que representa rfc.
        /// </summary>
[Display (Name ="RFC")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El valor para {0} debe ser de entre {2} y {1} caracteres.")]
        [RegularExpression(RegExp.RFCPattern, ErrorMessage = "El RFC no tiene un formato válido.")]
        public string? RFCFld { get; set; }

        //IdCtRegimenFiscal	smallint	2	yes
                /// <summary>
        /// Identificador único de id cat regimen fiscal.
        /// </summary>
[Display(Name = "Régimen Fiscal")]
        public short? IdCatRegimenFiscalFld { get; set; }

        // CalleFiscal varchar 100 yes no
                /// <summary>
        /// Propiedad que representa calle fiscal.
        /// </summary>
[Display(Name = "Calle")]
        [StringLength(100, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        public string? CalleFiscalFld { get; set; }

        // NumeroExteriorFiscal	varchar	25	yes
                /// <summary>
        /// Propiedad que representa numero exterior fiscal.
        /// </summary>
[Display(Name = "Número Exterior")]
        [StringLength(25, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        public string? NumeroExteriorFiscalFld{ get; set; }

        // NumeroInteriorFiscal	varchar	25	yes	no
                /// <summary>
        /// Propiedad que representa numero interior fiscal.
        /// </summary>
[Display(Name = "Número Interior")]
        [StringLength(25, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        public string? NumeroInteriorFiscalFld { get; set; }

        // CodigoPostalFiscal	char	5	yes	no
                /// <summary>
        /// Propiedad que representa codigo postal fiscal.
        /// </summary>
[Display(Name ="Código Postal ")]
        [StringLength(5, MinimumLength =5, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public string? CodigoPostalFiscalFld { get; set; }

        // ColoniaFiscal	varchar	50	yes	no
                /// <summary>
        /// Propiedad que representa colonia fiscal.
        /// </summary>
[Display(Name = "Colonia ")]
        [StringLength(50, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public string? ColoniaFiscalFld { get; set; }

        // LocalidadFiscal	varchar	50	yes	no
                /// <summary>
        /// Propiedad que representa localidad fiscal.
        /// </summary>
[Display(Name = "Localidad")]
        [StringLength(50, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]

        public string? LocalidadFiscalFld { get; set; }

        // MunicipioFiscal	varchar	50	yes	no
                /// <summary>
        /// Propiedad que representa municipio fiscal.
        /// </summary>
[Display(Name = "Municipio")]
        [StringLength(50, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public string? MunicipioFiscalFld { get; set; }

        // EntidadFederativaFiscal	varchar	50	yes	no
                /// <summary>
        /// Propiedad que representa entidad fiscal.
        /// </summary>
[Display(Name = "Entidad Federativa")]
        [StringLength(50, ErrorMessage = "La {0} no puede exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public string? EntidadFiscalFld { get; set; }

                /// <summary>
        /// Propiedad que representa limite credito semanal.
        /// </summary>
[Display(Name = "Límite de crédito semanal ($)")]
        [Range(0, 99999999, ErrorMessage = "El {0} debe ser mayor o igual a 0.")]
        public decimal LimiteCreditoSemanalFld { get; set; }
    }
}
