using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
    public class CrearCompraDto
    {

        #region Generales 

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required int CtProv_IdFld { get; set; }

        #endregion

        #region Producto
        public List<AgregarProductoCompraDto>? Productos { get; set; }

        #endregion

        #region Pago
        /* * El pago se maneja de forma independiente a la compra, pero se incluyen los campos necesarios para crear el pago al momento de crear la compra.
         */

[Display(Name = "Pago")]
[Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe de ser mayor o igual a {1}.")]
public decimal PagoFld { get; set; }

[Display(Name = "Medio de pago")]
[Required(ErrorMessage = "El valor para {0} es requerido.")]
public byte CtMediPago_IdFld { get; set; }

[Display(Name = "Notas")]
[StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
public string? NotasPagoFld { get; set; }

[Display(Name = "Cuenta bancaria")]
public byte? CtCuentaBancaria_IdFld { get; set; }

#endregion

#region Bitacora
public byte CtSucu_Id { get; set; }
public short CtUsua_Id { get; set; }

#endregion
}
}
