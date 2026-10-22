using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar compra.
    /// </summary>
public class ListarCompraDto: IIdDto<int>, IFolioDto
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

                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prov nombre.
        /// </summary>
public string CtProv_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa pagado.
        /// </summary>
public bool Pagado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total.
        /// </summary>
public decimal Total { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagado.
        /// </summary>
public decimal TotalPagado { get; set; }
    }
}
