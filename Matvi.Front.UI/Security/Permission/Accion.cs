namespace Matvi.Front.UI.Security.Permission
{
    /// <summary>
    /// Catálogo de identificadores de acciones de seguridad y permisos operativos del sistema.
    /// </summary>
    public class Accion
    {
        // General
        /// <summary>Acción general para crear registros.</summary>
        public const string Crear = "Crear";
        /// <summary>Acción general para editar o actualizar registros.</summary>
        public const string Editar = "Editar";
        /// <summary>Acción general para eliminar registros.</summary>
        public const string Eliminar = "Eliminar";

        // Presentaciones de productos
        /// <summary>Acción para crear presentaciones de producto.</summary>
        public const string Crear_PresentacionProducto = "Crear Presentación de Producto";
        /// <summary>Acción para modificar presentaciones de producto.</summary>
        public const string Editar_PresentacionProducto = "Editar Presentación de Producto";
        /// <summary>Acción para eliminar presentaciones de producto.</summary>
        public const string Eliminar_PresentacionProducto = "Eliminar Presentación de Producto";

        // Productos en sucursal
        /// <summary>Acción para asociar productos a una sucursal.</summary>
        public const string Crear_ProductoSucursal = "Crear Producto en Sucursal";
        /// <summary>Acción para editar configuración de producto en sucursal.</summary>
        public const string Editar_ProductoSucursal = "Editar Producto en Sucursal";
        /// <summary>Acción para desvincular un producto de una sucursal.</summary>
        public const string Eliminar_ProductoSucursal = "Eliminar Producto en Sucursal";

        // Lotes
        /// <summary>Acción para regularizar existencias de inventario en lotes.</summary>
        public const string Regularizar = "Regularizar";

        // Pedidos
        /// <summary>Acción para surtir mercancía de un pedido.</summary>
        public const string Surtir = "Surtir";
        /// <summary>Acción para marcar un pedido como entregado.</summary>
        public const string Entregar = "Entregar";
        /// <summary>Acción para cobrar un pedido.</summary>
        public const string Cobrar = "Cobrar";
        /// <summary>Acción para cancelar un pedido.</summary>
        public const string Cancelar = "Cancelar";
        /// <summary>Acción para ajustar rechazos de mercancía en pedidos.</summary>
        public const string AjustarRechazo = "AjustarRechazo";

        // Finanzas
        /// <summary>Acción para conciliar movimientos bancarios o contables.</summary>
        public const string Conciliar = "Conciliar";
    }
}
