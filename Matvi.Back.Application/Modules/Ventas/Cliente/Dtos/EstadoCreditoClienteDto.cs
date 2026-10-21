namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
    public class EstadoCreditoClienteDto
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public decimal LimiteCreditoSemanal { get; set; }
        public decimal DeudaActual { get; set; }
        public decimal CreditoDisponible { get; set; }
        public bool TieneAdeudoSemanaAnterior { get; set; }
        public bool AptoParaCredito { get; set; }
        public string MotivoEstado { get; set; } = string.Empty;
    }
}
