using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar pedido.
    /// </summary>
public class ListarPedidoDto : IIdDto<int>, IFolioDto
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
public string Folio => this.GetFolio(TipoConsecutivoEnm.PEDIDO);

        #endregion

                /// <summary>
        /// Texto descriptivo o detalle de ct clie nombre.
        /// </summary>
public string CtClie_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa ct clie telefono.
        /// </summary>
public string CtClie_Telefono { get; set; } = string.Empty;

                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de ct cana capt nombre.
        /// </summary>
public string CtCanaCapt_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Texto descriptivo o detalle de ct esta pedi nombre.
        /// </summary>
public string CtEstaPedi_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa entrega domicilio.
        /// </summary>
public bool EntregaDomicilio { get; set; }

                /// <summary>
        /// Monto económico correspondiente a total.
        /// </summary>
public decimal Total { get; set; }  
    }
}
