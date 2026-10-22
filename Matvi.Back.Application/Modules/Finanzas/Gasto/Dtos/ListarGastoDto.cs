using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Configurations;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar gasto.
    /// </summary>
public class ListarGastoDto: IIdDto<int>, IFolioDto
    {

        #region Implementación de IIdDto

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
public string Folio => this.GetFolio(TipoConsecutivoEnm.GASTO);

        #endregion
                /// <summary>
        /// Fecha y hora correspondiente a fecha pago.
        /// </summary>
public DateTime FechaPago { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct tipo gast nombre.
        /// </summary>
public string CtTipoGast_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa proveedor.
        /// </summary>
public string Proveedor { get; set; } = string.Empty;
                /// <summary>
        /// Monto económico correspondiente a importe.
        /// </summary>
public decimal Importe { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct medio pago nombre.
        /// </summary>
public string CtMedioPago_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct usua nombre.
        /// </summary>
public string CtUsua_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string Notas { get; set; } = string.Empty;

    }
}
