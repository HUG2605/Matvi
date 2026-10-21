using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Dtos
{
    public class DetallarGrupoDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string CtSecc_Nombre { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
