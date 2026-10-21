using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos
{
    public class ListarEmpresaDto : IIdDto<byte>
    {
        #region Implementacion de IIdDto

        public byte Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;

        public string? Telefono { get; set; }
    }
}
