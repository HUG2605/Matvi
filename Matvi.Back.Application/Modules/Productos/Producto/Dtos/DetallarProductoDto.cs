 using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar producto.
    /// </summary>
public class DetallarProductoDto : IIdDto<int>, IProductoBaseDto
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

        #region implementacion de IProductoBaseDto

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
[Display(Name = "Producto")]
        public string CtProd_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa ct prod especificacion tecnica.
        /// </summary>
public string? CtProd_EspecificacionTecnica { get; set; }
                /// <summary>
        /// Propiedad que representa ct prod marca.
        /// </summary>
public string? CtProd_Marca { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre completo.
        /// </summary>
public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

                /// <summary>
        /// Texto descriptivo o detalle de ct secc nombre.
        /// </summary>
public string CtSecc_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct grup nombre.
        /// </summary>
public string CtGrup_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct grup descripcion.
        /// </summary>
public string CtGrup_Descripcion { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct unid base inve nombre.
        /// </summary>
public string CtUnidBaseInve_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa bajo pedido.
        /// </summary>
public bool BajoPedido { get; set; }
                /// <summary>
        /// Propiedad que representa entrega mostrador.
        /// </summary>
public bool EntregaMostrador { get; set; }
                /// <summary>
        /// Propiedad que representa imagen.
        /// </summary>
public string? Imagen { get; set; }
                /// <summary>
        /// Propiedad que representa codigo barras interno.
        /// </summary>
public bool CodigoBarrasInterno { get; set; }
                /// <summary>
        /// Propiedad que representa codigo barras.
        /// </summary>
public string? CodigoBarras { get; set; }
                /// <summary>
        /// Indica si activo se encuentra activo o habilitado.
        /// </summary>
public bool Activo { get; set; }
                /// <summary>
        /// Colección de elementos asociados a proveedores.
        /// </summary>
public List<RelacionarProveedorDto>? Proveedores { get; set; }
    }
}
