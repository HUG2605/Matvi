using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
    public class DetallarClienteDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
        public string? Alias { get; set; } = string.Empty;
        public string? Direccion { get; internal set; }
        public string Telefono { get; set; } = string.Empty;
        public string? CtRegiFisc_Nombre { get; set; }=string.Empty;
        public string? RFCFiscal { get; set; } = string.Empty;
        public string? CalleFiscal { get; set; } = string.Empty;
        public string? NumeroExteriorFiscal { get; set; } = string.Empty;
        public string? NumeroInteriorFiscal { get; set; }=string.Empty;
        public string? CodigoPostalFiscal { get; set; }= string.Empty;
        public string? ColoniaFiscal { get; set; } = string.Empty;
        public string? LocalidadFiscal { get; set; } = string.Empty;
        public string? MunicipioFiscal { get; set; } = string.Empty;
        public string? EntidadFederativaFiscal { get; set; } = string.Empty;
        public decimal LimiteCreditoSemanal { get; set; }
    }
}
