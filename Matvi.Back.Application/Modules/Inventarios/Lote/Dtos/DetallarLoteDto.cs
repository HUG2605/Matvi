using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar lote.
    /// </summary>
public class DetallarLoteDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
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

        #region Implementacion de IFolioDto

                /// <summary>
        /// Propiedad que representa ct sucu clave.
        /// </summary>
public string CtSucu_Clave { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero.
        /// </summary>
public int Numero { get; set; }
                /// <summary>
        /// Propiedad que representa folio.
        /// </summary>
public string Folio => this.GetFolio(TipoConsecutivoEnm.COMPRA);

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
        /// Texto descriptivo o detalle de ct sucu nombre.
        /// </summary>
public string CtSucu_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Identificador único de ct tipo lote id.
        /// </summary>
public byte CtTipoLote_Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct tipo lote nombre.
        /// </summary>
public string CtTipoLote_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad inicial ubi.
        /// </summary>
public decimal CantidadInicialUBI { get; set; }
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad disponible ubi.
        /// </summary>
public decimal CantidadDisponibleUBI { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo unitario.
        /// </summary>
public decimal CostoUnitario { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo unitario base.
        /// </summary>
public decimal CostoUnitarioBase { get; set; }
                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

    }
}
