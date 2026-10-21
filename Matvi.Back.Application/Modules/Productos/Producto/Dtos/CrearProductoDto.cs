using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
    public class CrearProductoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtSecc_IdFld { get; set; }

        //IdCatGrupoProducto    short	 1	no
        [Display(Name = "Grupo")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required short CtGrup_IdFld { get; set; }

        //Nombre varchar	50	no
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //EspecificacionTecnica varchar	50	yes
        [Display(Name = "Especificación técnica")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? EspecificacionTecnicaFld { get; set; }

        //Marca varchar	50	yes
        [Display(Name = "Marca")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? MarcaFld { get; set; }

        //IdCtUnidadMedidaBase    tinyint	 1	no
        [Display(Name = "Unidad de medida base de inventario")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtUnidBaseInve_IdFld { get; set; }

        //Imagen varchar	100	yes
        public string? ImagenFld { get; set; }

        public bool CodigoBarrasInternoFld { get; set; }

        //Código varchar	20	yes
        [Display(Name = "Código de barras")]
        [StringLength(20, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? CodigoBarrasFld { get; set; }

        //Activo bit	 1	no
        [Display(Name = "Activo")]
        public bool ActivoFld { get; set; }

        //BajoPedido: permite pedir sin stock
        [Display(Name = "Bajo pedido")]
        public bool BajoPedidoFld { get; set; }

        //EntregaMostrador: el vendedor puede entregar directamente
        [Display(Name = "Entrega en mostrador")]
        public bool EntregaMostradorFld { get; set; }

        public List<RelacionarProveedorDto>? Proveedores { get; set; }

    }
}
