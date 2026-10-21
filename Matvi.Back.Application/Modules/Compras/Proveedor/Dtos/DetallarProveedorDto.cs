using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Dtos
{
    public class DetallarProveedorDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreContacto { get; set; } = string.Empty;
        public string TelefonoContacto { get; set; } = string.Empty;
        public List<string>? Productos { get; set; }

    }
}
