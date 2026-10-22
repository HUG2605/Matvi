# Documentación Técnica del Proyecto Matvi

Bienvenido al portal de documentación técnica y referencia de API del sistema **Matvi**.

---

## 🏛️ Estructura y Capas de la Solución

El sistema está desarrollado en **ASP.NET Core 10.0** adoptando patrones de **Clean Architecture** y **CQRS** (Command Query Responsibility Segregation) con **MediatR** y **Entity Framework Core**:

### 1. Matvi.Transversal
Capa compartida con excepciones globales y contratos base reutilizables entre todas las capas del sistema.

### 2. Matvi.Back.Infrastructure
Capa de infraestructura y acceso a datos:
- **Unit of Work y Repositorios:** Coordinación transaccional y persistencia.
- **Dual DbContext:** `MatviCmdDbCtx` para operaciones de escritura con auditoría automática (`AuditInterceptor`), y `MatviQryDbCtx` optimizado para consultas de lectura sin seguimiento.
- **Consecutivos y Notificaciones:** Asignación atómica de números de folio por sucursal y notificaciones push web vía VAPID.

### 3. Matvi.Back.Application
Capa de lógica de negocio organizada por módulos funcionales:
- **Compras:** Proveedores y órdenes de compra.
- **Configuración:** Empresas y sucursales.
- **Finanzas:** Cobros, pagos, gastos, cortes de caja y movimientos bancarios/efectivo.
- **Inventarios:** Control de inventario, lotes y movimientos.
- **Productos:** Catálogo de productos, presentaciones, grupos y secciones.
- **Seguridad:** Autenticación, usuarios y roles.
- **Ventas:** Clientes, pedidos y flujo de surtido/entrega.

### 4. Matvi.Front.UI
Capa de presentación web basada en Razor Pages, controladores de API y middleware de autenticación por cookies y autorización basada en matrices de permisos (Función y Acción).

---

## 📚 Explorar la API
Para consultar la especificación detallada de clases, interfaces, métodos y propiedades, accede a la sección de [**API**](api/index.md).
