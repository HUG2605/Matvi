using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class CerrarCorteCajaResDto
    {
        public decimal TotalCobros { get; set; }
        public decimal TotalEntregasRepartidor { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalPagos { get; set; }
        public decimal FondoFinalSistema { get; set; }
        public decimal FondoFinalContado { get; set; }
        public decimal Diferencia { get; set; }
    }
}