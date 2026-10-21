using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
    public class ListarProductoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
        public string? EspecificacionTecnica { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string CtSecc_Nombre { get; set; } = string.Empty;
        public string CtGrup_Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
