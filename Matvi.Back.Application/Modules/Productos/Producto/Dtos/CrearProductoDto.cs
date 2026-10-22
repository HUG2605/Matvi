using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear producto.
    /// </summary>
public class CrearProductoDto : IIdDto<int>
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
        /// Propiedad que representa ct secc id.
        /// </summary>
[Display(Name = "Sección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtSecc_IdFld { get; set; }

        //IdCatGrupoProducto    short	 1	no
                /// <summary>
        /// Propiedad que representa ct grup id.
        /// </summary>
[Display(Name = "Grupo")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required short CtGrup_IdFld { get; set; }

        //Nombre varchar	50	no
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //EspecificacionTecnica varchar	50	yes
                /// <summary>
        /// Indica si especificacion tecnica se encuentra activo o habilitado.
        /// </summary>
[Display(Name = "Especificación técnica")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? EspecificacionTecnicaFld { get; set; }

        //Marca varchar	50	yes
                /// <summary>
        /// Propiedad que representa marca.
        /// </summary>
[Display(Name = "Marca")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? MarcaFld { get; set; }

        //IdCtUnidadMedidaBase    tinyint	 1	no
                /// <summary>
        /// Propiedad que representa ct unid base inve id.
        /// </summary>
[Display(Name = "Unidad de medida base de inventario")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtUnidBaseInve_IdFld { get; set; }

        //Imagen varchar	100	yes
                /// <summary>
        /// Propiedad que representa imagen.
        /// </summary>
public string? ImagenFld { get; set; }

                /// <summary>
        /// Propiedad que representa codigo barras interno.
        /// </summary>
public bool CodigoBarrasInternoFld { get; set; }

        //Código varchar	20	yes
                /// <summary>
        /// Propiedad que representa codigo barras.
        /// </summary>
[Display(Name = "Código de barras")]
        [StringLength(20, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? CodigoBarrasFld { get; set; }

        //Activo bit	 1	no
                /// <summary>
        /// Propiedad que representa activo.
        /// </summary>
[Display(Name = "Activo")]
        public bool ActivoFld { get; set; }

        //BajoPedido: permite pedir sin stock
                /// <summary>
        /// Propiedad que representa bajo pedido.
        /// </summary>
[Display(Name = "Bajo pedido")]
        public bool BajoPedidoFld { get; set; }

        //EntregaMostrador: el vendedor puede entregar directamente
                /// <summary>
        /// Propiedad que representa entrega mostrador.
        /// </summary>
[Display(Name = "Entrega en mostrador")]
        public bool EntregaMostradorFld { get; set; }

                /// <summary>
        /// Colección de elementos asociados a proveedores.
        /// </summary>
public List<RelacionarProveedorDto>? Proveedores { get; set; }

    }
}
