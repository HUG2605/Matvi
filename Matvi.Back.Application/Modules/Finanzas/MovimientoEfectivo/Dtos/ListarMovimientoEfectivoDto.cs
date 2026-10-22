using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar movimiento efectivo.
    /// </summary>
public class ListarMovimientoEfectivoDto : IIdDto<int>
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
        /// Texto descriptivo o detalle de origen descripcion.
        /// </summary>
public string OrigenDescripcion { get; set; } = string.Empty;
                /// <summary>
        /// Monto económico correspondiente a monto.
        /// </summary>
public decimal Monto { get; set; }
                /// <summary>
        /// Propiedad que representa cajero.
        /// </summary>
public string Cajero { get; set; } = string.Empty;

        // Corte de Caja
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
        /// Propiedad que representa en corte.
        /// </summary>
public bool EnCorte => IdOpCorteCaja.HasValue;
    }
}
