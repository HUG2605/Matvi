using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos
{
    public class DetallarMovimientoBancarioDto : IIdDto<int>
    {
        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        public DateTime Fecha { get; set; }
        public byte Tipo { get; set; } // 1: Ingreso, 2: Egreso
        public string TipoDescripcion => Tipo == 1 ? "Ingreso" : "Egreso";
        public decimal Monto { get; set; }
        public string? Referencia { get; set; }
        public bool Conciliado { get; set; }

        // Cuenta Bancaria
        public byte IdCtCuentaBancaria { get; set; }
        public string Banco { get; set; } = string.Empty;
        public string NumeroCuenta { get; set; } = string.Empty;
        public string CuentaNombre { get; set; } = string.Empty;

        // Origen
        public string OrigenTipo { get; set; } = string.Empty; // "Cobro", "Gasto", "Pago", "Manual"
        public int? OrigenId { get; set; }
        public string? OrigenSafeId => OrigenId.HasValue ? OrigenId.Value.GetSafeId() : null;
        public int? OrigenNumero { get; set; }
        public string? OrigenSucuClave { get; set; }
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
        public string? OrigenUrl
        {
            get
            {
                if (!OrigenId.HasValue || string.IsNullOrEmpty(OrigenSafeId)) return null;
                return OrigenTipo switch
                {
                    "Cobro" => $"/Cobro/Details?id={OrigenSafeId}",
                    "Gasto" => $"/Gasto/Details?id={OrigenSafeId}",
                    "Pago" => $"/Pago/Details?id={OrigenSafeId}",
                    _ => null
                };
            }
        }
        public string? OrigenConcepto { get; set; }
        public string? OrigenTercero { get; set; } // Nombre de cliente / proveedor
        public DateTime? OrigenFecha { get; set; }
        public decimal? OrigenMonto { get; set; }
        public string? OrigenNotas { get; set; }
        public string? OrigenUsuario { get; set; }
        public string? SucursalClave { get; set; }
        public string? SucursalNombre { get; set; }
    }
}
