using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear compra.
    /// </summary>
public class CrearCompraDto
    {

        #region Generales 

                /// <summary>
        /// Propiedad que representa ct prov id.
        /// </summary>
[Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required int CtProv_IdFld { get; set; }

        #endregion

        #region Producto
                /// <summary>
        /// Colección de elementos asociados a productos.
        /// </summary>
public List<AgregarProductoCompraDto>? Productos { get; set; }

        #endregion

        #region Pago
        /* * El pago se maneja de forma independiente a la compra, pero se incluyen los campos necesarios para crear el pago al momento de crear la compra.
         */

/// <summary>
/// Propiedad que representa pago.
/// </summary>
[Display(Name = "Pago")]
[Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe de ser mayor o igual a {1}.")]
public decimal PagoFld { get; set; }

/// <summary>
/// Propiedad que representa ct medi pago id.
/// </summary>
[Display(Name = "Medio de pago")]
[Required(ErrorMessage = "El valor para {0} es requerido.")]
public byte CtMediPago_IdFld { get; set; }

/// <summary>
/// Propiedad que representa notas pago.
/// </summary>
[Display(Name = "Notas")]
[StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
public string? NotasPagoFld { get; set; }

/// <summary>
/// Propiedad que representa ct cuenta bancaria id.
/// </summary>
[Display(Name = "Cuenta bancaria")]
public byte? CtCuentaBancaria_IdFld { get; set; }

#endregion

#region Bitacora
/// <summary>
/// Identificador único de ct sucu id.
/// </summary>
public byte CtSucu_Id { get; set; }
/// <summary>
/// Identificador único de ct usua id.
/// </summary>
public short CtUsua_Id { get; set; }

#endregion
}
}
