using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
    public class ListarMovimientoEfectivoDto : IIdDto<int>
    {
        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        public DateTime Fecha { get; set; }
        public byte Tipo { get; set; } // 1: Ingreso, 2: Egreso
        public string TipoDescripcion => Tipo == 1 ? "Ingreso" : "Egreso";

        public string OrigenTipo { get; set; } = string.Empty; // "Cobro", "Entrega Repartidor", "Gasto", "Pago"
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

        public string OrigenDescripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Cajero { get; set; } = string.Empty;

        // Corte de Caja
        public int? IdOpCorteCaja { get; set; }
        public string? CorteCajaFolio { get; set; }
        public string? CorteCajaSafeId => IdOpCorteCaja.HasValue ? IdOpCorteCaja.Value.GetSafeId() : null;
        public bool EnCorte => IdOpCorteCaja.HasValue;
    }
}
