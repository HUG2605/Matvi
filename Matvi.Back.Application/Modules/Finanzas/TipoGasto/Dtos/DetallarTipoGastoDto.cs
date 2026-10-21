using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos
{
    public class DetallarTipoGastoDto : IIdDto<byte>
    {
        #region Implementacion de IIdDto

        public byte Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
    }
}
