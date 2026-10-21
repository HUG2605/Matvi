namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
    public class ResumenEfectivoDto
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal FlujoNeto => TotalIngresos - TotalEgresos;
        public int TotalSinCorte { get; set; }
        public decimal MontoSinCorte { get; set; }
        public int TotalMovimientos { get; set; }
    }
}
