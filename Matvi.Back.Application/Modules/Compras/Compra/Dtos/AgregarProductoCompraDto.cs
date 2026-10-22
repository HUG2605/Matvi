using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para agregar producto compra.
    /// </summary>
public class AgregarProductoCompraDto : IIdDto<int>, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get ; set ; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IProductoBaseDto
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

        #region Implementacion de IPresentacionProductoBaseDto

                /// <summary>
        /// Propiedad que representa ct pres prod id ct producto.
        /// </summary>
public int CtPresProd_IdCtProducto { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de ct pres prod nombre.
        /// </summary>
public string CtPresProd_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa ct pres prod factor ubi.
        /// </summary>
public decimal CtPresProd_FactorUBI { get; set; }

        #endregion

                /// <summary>
        /// Propiedad que representa ct pres prod id.
        /// </summary>
public int CtPresProd_IdFld { get; set; }
                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad.
        /// </summary>
public decimal CantidadFld { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo up.
        /// </summary>
public decimal CostoUP { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo ubi.
        /// </summary>
public decimal CostoUBI { get; set; }
                /// <summary>
        /// Monto económico correspondiente a importe.
        /// </summary>
public decimal Importe { get; set; }

    }
}
