using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
    public class DetallarMovimientoEfectivoDto : IIdDto<int>
    {
        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        public DateTime Fecha { get; set; }
        public byte Tipo { get; set; } // 1: Ingreso, 2: Egreso
        public string TipoDescripcion => Tipo == 1 ? "Ingreso" : "Egreso";

        public string OrigenTipo { get; set; } = string.Empty; // "Cobro", "Entrega Repartidor", "Gasto", "Pago"
        public int OrigenId { get; set; }
        public string OrigenSafeId => OrigenId.GetSafeId();
        public int OrigenNumero { get; set; }
        public string OrigenSucuClave { get; set; } = string.Empty;

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

        public string OrigenConcepto { get; set; } = string.Empty;
        public string? OrigenTercero { get; set; } // Nombre del cliente / proveedor / repartidor
        public decimal Monto { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string SucursalClave { get; set; } = string.Empty;
        public string SucursalNombre { get; set; } = string.Empty;
        public string? Notas { get; set; }

        // Datos del Corte de Caja
        public int? IdOpCorteCaja { get; set; }
        public string? CorteCajaFolio { get; set; }
        public string? CorteCajaSafeId => IdOpCorteCaja.HasValue ? IdOpCorteCaja.Value.GetSafeId() : null;
        public string? CorteCajaUrl => IdOpCorteCaja.HasValue ? $"/CorteCaja/Details?id={CorteCajaSafeId}" : null;
        public DateTime? CorteCajaFechaApertura { get; set; }
        public DateTime? CorteCajaFechaCierre { get; set; }
        public string? CorteCajaEstado { get; set; }
    }
}
