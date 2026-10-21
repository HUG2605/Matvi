namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos
{
    public class ResumenBancarioDto
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal BalanceNeto => TotalIngresos - TotalEgresos;
        public int TotalPendientesConciliar { get; set; }
        public decimal MontoPendienteConciliar { get; set; }
        public int TotalMovimientos { get; set; }
    }
}
