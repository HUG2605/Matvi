namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para estado credito cliente.
    /// </summary>
public class EstadoCreditoClienteDto
    {
                /// <summary>
        /// Identificador único de id cliente.
        /// </summary>
public int IdCliente { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de nombre cliente.
        /// </summary>
public string NombreCliente { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa limite credito semanal.
        /// </summary>
public decimal LimiteCreditoSemanal { get; set; }
                /// <summary>
        /// Propiedad que representa deuda actual.
        /// </summary>
public decimal DeudaActual { get; set; }
                /// <summary>
        /// Propiedad que representa credito disponible.
        /// </summary>
public decimal CreditoDisponible { get; set; }
                /// <summary>
        /// Propiedad que representa tiene adeudo semana anterior.
        /// </summary>
public bool TieneAdeudoSemanaAnterior { get; set; }
                /// <summary>
        /// Propiedad que representa apto para credito.
        /// </summary>
public bool AptoParaCredito { get; set; }
                /// <summary>
        /// Propiedad que representa motivo estado.
        /// </summary>
public string MotivoEstado { get; set; } = string.Empty;
    }
}
