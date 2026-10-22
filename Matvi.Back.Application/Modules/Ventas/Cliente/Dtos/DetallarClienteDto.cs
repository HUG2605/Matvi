using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar cliente.
    /// </summary>
public class DetallarClienteDto : IIdDto<int>
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
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa alias.
        /// </summary>
public string? Alias { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
public string? Direccion { get; internal set; }
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct regi fisc nombre.
        /// </summary>
public string? CtRegiFisc_Nombre { get; set; }=string.Empty;
                /// <summary>
        /// Propiedad que representa rfc fiscal.
        /// </summary>
public string? RFCFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa calle fiscal.
        /// </summary>
public string? CalleFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero exterior fiscal.
        /// </summary>
public string? NumeroExteriorFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero interior fiscal.
        /// </summary>
public string? NumeroInteriorFiscal { get; set; }=string.Empty;
                /// <summary>
        /// Propiedad que representa codigo postal fiscal.
        /// </summary>
public string? CodigoPostalFiscal { get; set; }= string.Empty;
                /// <summary>
        /// Propiedad que representa colonia fiscal.
        /// </summary>
public string? ColoniaFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa localidad fiscal.
        /// </summary>
public string? LocalidadFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa municipio fiscal.
        /// </summary>
public string? MunicipioFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa entidad federativa fiscal.
        /// </summary>
public string? EntidadFederativaFiscal { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa limite credito semanal.
        /// </summary>
public decimal LimiteCreditoSemanal { get; set; }
    }
}
