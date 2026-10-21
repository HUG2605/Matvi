using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos
{
    public class ListarSucursalDto : IIdDto<byte>
    {
        #region Implementacion de IIdDto

        public byte Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

    }
}
