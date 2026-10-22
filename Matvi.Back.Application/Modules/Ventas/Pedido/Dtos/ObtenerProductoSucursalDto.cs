namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para obtener producto sucursal.
    /// </summary>
public class ObtenerProductoSucursalDto
    {
        /// <summary>
        /// Indica si el producto se encuentra regularizado en la sucursal.
        /// </summary>
        public bool RlProdSucu_Regularizado { get; set; }

                /// <summary>
        /// Propiedad que representa regularizado.
        /// </summary>
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
