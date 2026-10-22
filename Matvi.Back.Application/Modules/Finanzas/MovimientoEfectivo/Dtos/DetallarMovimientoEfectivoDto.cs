using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar movimiento efectivo.
    /// </summary>
public class DetallarMovimientoEfectivoDto : IIdDto<int>
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Propiedad que representa tipo.
        /// </summary>
public byte Tipo { get; set; } // 1: Ingreso, 2: Egreso
                /// <summary>
        /// Texto descriptivo o detalle de tipo descripcion.
        /// </summary>
public string TipoDescripcion => Tipo == 1 ? "Ingreso" : "Egreso";

                /// <summary>
        /// Propiedad que representa origen tipo.
        /// </summary>
public string OrigenTipo { get; set; } = string.Empty; // "Cobro", "Entrega Repartidor", "Gasto", "Pago"
                /// <summary>
        /// Identificador único de origen id.
        /// </summary>
public int OrigenId { get; set; }
                /// <summary>
        /// Identificador único de origen safe id.
        /// </summary>
public string OrigenSafeId => OrigenId.GetSafeId();
                /// <summary>
        /// Propiedad que representa origen numero.
        /// </summary>
public int OrigenNumero { get; set; }
                /// <summary>
        /// Propiedad que representa origen sucu clave.
        /// </summary>
public string OrigenSucuClave { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa origen folio.
        /// </summary>
public string OrigenFolio
        {
            get
            {
                var num = OrigenNumero.ToString().PadLeft(5, '0');
                var prefijo = OrigenTipo switch
                {
                    "Cobro" => "COB",
                    "Entrega Repartidor" => "ENT",
                    "Gasto" => "GAS",
                    "Pago" => "PAG",
                    _ => "EFE"
                };
                return $"{OrigenSucuClave}{prefijo}{num}";
            }
        }

                /// <summary>
        /// Propiedad que representa origen url.
        /// </summary>
public string? OrigenUrl
        {
            get
            {
                return OrigenTipo switch
                {
                    "Cobro" => $"/Cobro/Details?id={OrigenSafeId}",
                    "Gasto" => $"/Gasto/Details?id={OrigenSafeId}",
                    "Pago" => $"/Pago/Details?id={OrigenSafeId}",
                    _ => null
                };
            }
        }

                /// <summary>
        /// Propiedad que representa origen concepto.
        /// </summary>
public string OrigenConcepto { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa origen tercero.
        /// </summary>
public string? OrigenTercero { get; set; } // Nombre del cliente / proveedor / repartidor
                /// <summary>
        /// Monto económico correspondiente a monto.
        /// </summary>
public decimal Monto { get; set; }
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
public string Usuario { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa sucursal clave.
        /// </summary>
public string SucursalClave { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de sucursal nombre.
        /// </summary>
public string SucursalNombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string? Notas { get; set; }

        // Datos del Corte de Caja
                /// <summary>
        /// Identificador único de id op corte caja.
        /// </summary>
public int? IdOpCorteCaja { get; set; }
                /// <summary>
        /// Propiedad que representa corte caja folio.
        /// </summary>
public string? CorteCajaFolio { get; set; }
                /// <summary>
        /// Identificador único de corte caja safe id.
        /// </summary>
public string? CorteCajaSafeId => IdOpCorteCaja.HasValue ? IdOpCorteCaja.Value.GetSafeId() : null;
                /// <summary>
        /// Propiedad que representa corte caja url.
        /// </summary>
public string? CorteCajaUrl => IdOpCorteCaja.HasValue ? $"/CorteCaja/Details?id={CorteCajaSafeId}" : null;
                /// <summary>
        /// Fecha y hora correspondiente a corte caja fecha apertura.
        /// </summary>
public DateTime? CorteCajaFechaApertura { get; set; }
                /// <summary>
        /// Fecha y hora correspondiente a corte caja fecha cierre.
        /// </summary>
public DateTime? CorteCajaFechaCierre { get; set; }
                /// <summary>
        /// Propiedad que representa corte caja estado.
        /// </summary>
public string? CorteCajaEstado { get; set; }
    }
}
