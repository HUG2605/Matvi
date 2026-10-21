using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
    public class ListarClienteDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public decimal LimiteCreditoSemanal { get; set; }
    }
}
