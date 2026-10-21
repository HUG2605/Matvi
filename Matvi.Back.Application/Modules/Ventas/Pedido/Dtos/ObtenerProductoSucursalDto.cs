namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class ObtenerProductoSucursalDto
    {
        /// <summary>
        /// Indica si el producto se encuentra regularizado en la sucursal.
        /// </summary>
        public bool RlProdSucu_Regularizado { get; set; }

        public bool Regularizado
        {
            get => RlProdSucu_Regularizado;
            set => RlProdSucu_Regularizado = value;
        }

        /// <summary>
        /// Permite pedir el producto aunque no haya existencia.
        /// </summary>
        public bool CtProd_BajoPedido { get; set; }
    }
}
