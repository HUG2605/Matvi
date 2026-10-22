using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar producto sucursal.
    /// </summary>
public class DetallarProductoSucursalDto : IProductoBaseDto
    {
                /// <summary>
        /// Identificador único de ct suc id.
        /// </summary>
public byte CtSuc_Id { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de ct suc nombre.
        /// </summary>
public string CtSuc_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Identificador único de ct prod id.
        /// </summary>
public int CtProd_Id { get; set; }

                /// <summary>
        /// Identificador único de ct prod safe id.
        /// </summary>
public string CtProd_SafeId => CtProd_Id.ToString()?.ToObfuscated() ?? string.Empty;

        #region implementacion de IProductoBaseDto

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
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
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

                /// <summary>
        /// Cantidad o volumen de unidades de stock minimo ubi.
        /// </summary>
public decimal StockMinimoUBI { get; set; }

                /// <summary>
        /// Propiedad que representa existencias ubi.
        /// </summary>
public decimal ExistenciasUBI { get; set; }

                /// <summary>
        /// Cantidad o volumen de unidades de stock maximo ubi.
        /// </summary>
public decimal StockMaximoUBI { get; set; }

                /// <summary>
        /// Propiedad que representa regularizado.
        /// </summary>
public bool Regularizado { get; set; }

                /// <summary>
        /// Propiedad que representa ubicacion.
        /// </summary>
public string? Ubicacion { get; set; }
    }
}
