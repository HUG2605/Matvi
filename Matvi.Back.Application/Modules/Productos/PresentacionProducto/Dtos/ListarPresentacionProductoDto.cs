using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos
{
    public class ListarProductoSucursalDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public string CtPresProd_Descripcion { get; set; } = string.Empty;

        //EsParaCompra    bit	1	no
        public bool EsParaCompra { get; set; }

        //EsParaVenta bit	1	no
        public bool EsParaVenta { get; set; }

        //PrecioVenta decimal	9	no
        public decimal PrecioVenta { get; set; }

        //PrecioMayoreo   decimal	9	yes
        public decimal? PrecioMayoreo { get; set; }

        //UnidadesMayoreo smallint	1	yes
        public short? UnidadesMayoreo { get; set; }

        public bool EsPresentacionBase { get; set; }

    }
}
