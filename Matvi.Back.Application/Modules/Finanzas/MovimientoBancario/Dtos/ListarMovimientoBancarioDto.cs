using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar movimiento bancario.
    /// </summary>
public class ListarMovimientoBancarioDto : IIdDto<int>
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
        /// Monto económico correspondiente a monto.
        /// </summary>
public decimal Monto { get; set; }
                /// <summary>
        /// Propiedad que representa referencia.
        /// </summary>
public string? Referencia { get; set; }
                /// <summary>
        /// Propiedad que representa conciliado.
        /// </summary>
public bool Conciliado { get; set; }

                /// <summary>
        /// Identificador único de id ct cuenta bancaria.
        /// </summary>
public byte IdCtCuentaBancaria { get; set; }
                /// <summary>
        /// Propiedad que representa banco.
        /// </summary>
public string Banco { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero cuenta.
        /// </summary>
public string NumeroCuenta { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de cuenta nombre.
        /// </summary>
public string CuentaNombre { get; set; } = string.Empty;

        // Origen
                /// <summary>
        /// Propiedad que representa origen tipo.
        /// </summary>
public string OrigenTipo { get; set; } = string.Empty; // "Cobro", "Gasto", "Pago", "Manual"
                /// <summary>
        /// Propiedad que representa origen numero.
        /// </summary>
public int? OrigenNumero { get; set; }
                /// <summary>
        /// Propiedad que representa origen sucu clave.
        /// </summary>
public string? OrigenSucuClave { get; set; }
                /// <summary>
        /// Propiedad que representa origen folio.
        /// </summary>
public string? OrigenFolio
        {
            get
            {
                if (!OrigenNumero.HasValue) return null;
                var num = OrigenNumero.Value.ToString().PadLeft(5, '0');
                var prefijo = OrigenTipo switch
                {
                    "Cobro" => "COB",
                    "Gasto" => "GAS",
                    "Pago" => "PAG",
                    _ => "MOV"
                };
                return $"{OrigenSucuClave}{prefijo}{num}";
            }
        }
                /// <summary>
        /// Identificador único de origen id.
        /// </summary>
public int? OrigenId { get; set; }
                /// <summary>
        /// Identificador único de origen safe id.
        /// </summary>
public string? OrigenSafeId => OrigenId.HasValue ? OrigenId.Value.GetSafeId() : null;
                /// <summary>
        /// Texto descriptivo o detalle de origen descripcion.
        /// </summary>
public string? OrigenDescripcion { get; set; }
                /// <summary>
        /// Propiedad que representa sucursal clave.
        /// </summary>
public string? SucursalClave { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de sucursal nombre.
        /// </summary>
public string? SucursalNombre { get; set; }
    }
}
