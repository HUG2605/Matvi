namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para editar producto.
    /// </summary>
public class EditarProductoDto : CrearProductoDto
    {
                /// <summary>
        /// Texto descriptivo o detalle de ct unid base inve nombre.
        /// </summary>
public string CtUnidBaseInve_Nombre { get; set; } = string.Empty;
    }
}
