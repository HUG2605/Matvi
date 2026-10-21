# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run
```bash
dotnet build Matvi.sln
dotnet run --project Matvi.Front.UI
```
Dev server: `http://localhost:5031` | `https://localhost:7022`

No automated test project exists in the solution. Validate changes by building and exercising the relevant module in the running app.

## Architecture

**ASP.NET Core 10.0 Razor Pages** with Areas + CQRS (MediatR) + EF Core (SQL Server).

### Project Layout
```
Matvi.Front.UI/           → Web layer (Razor Pages, Controllers, JS, Areas)
Matvi.Back.Application/   → Business logic (Commands, Queries, Handlers, DTOs)
Matvi.Back.Infrastructure/→ Data access (EF Core, DbContexts, Repositories, Interceptors)
Matvi.Transversal/        → Shared exceptions
```

DI is wired in `Program.cs` (UI), `DiApplication.cs` (Application), and `DiInfrastructure.cs` (Infrastructure).

### Module Organization
Modules: `Compras`, `Configuracion`, `Finanzas`, `Inventarios`, `Productos`, `Seguridad`, `Ventas`. Each module follows:
- **Frontend**: `Matvi.Front.UI/Areas/{Module}/Pages/{Entity}/` (CRUD pages) + `Matvi.Front.UI/Areas/{Module}/Api/` (controllers)
- **Backend**: `Matvi.Back.Application/Modules/{Module}/{Entity}/` with `Commands/`, `Queries/`, `Handlers/`, `Dtos/`
- **JS**: `Matvi.Front.UI/wwwroot/js/Modules/{Module}/{Entity}/` (e.g., `Create.js`, `Index.js`)

### Key Patterns

**Dual DbContext** (CQRS split):
- `MatviCmdDbCtx` (scoped, with `AuditInterceptor`) → Commands
- `MatviQryDbCtx` (created via `DbContextFactory`, no interceptors) → Queries

**Authorization:** `[AuthorizeMatvi(Funcion.X, Accion.Y)]` attribute on PageModels.
- `Funcion.cs` → module permissions (e.g., `Compras_Compras`, `Ventas_Pedidos`)
- `Accion.cs` → actions (`Crear`, `Editar`, `Eliminar`, `Surtir`, `Regularizar`, `Entregar`, `Cobrar`, `Cancelar`, `AjustarRechazo`)

**Transactional handlers & MediatR (PITFALL — READ FIRST):**
- A handler marked with `[Transaction]` is wrapped by `TransactionalCmdHandlerDecorator`, which calls `SaveChanges`/`CommitAsync` on the **shared scoped** `MatviCmdDbCtx` when it returns.
- **Never** invoke another `[Transaction]` handler via `_sender.Send(...)` from inside a `[Transaction]` handler. The nested decorator commits the shared transaction prematurely, and any later `ConsecutivoService.GenerarNumeroAsync(...)` call (which runs `sp_getapplock` with `LockOwner='Transaction'`) fails with **-999**.
- Correct pattern: register the inner handler as a concrete dependency in `DiApplication.AddApplication` (e.g. `services.AddScoped<AjustarEstatusPedidoCmdHdl>();`) and call it directly. See `CrearPedidoCmdHdl` → `_ajustarEstatusHdl.Handle(...)`.
- Queries (`*Qry`) with `QryDbCtx` factory are safe to invoke via `_sender.Send` from inside a transactional handler — they use a separate context with no transaction.

**PageModel base:** Inherit `BaseModel` (not `PageModel`) for obfuscation helpers.

**Grid pattern:** DataTables with `OnGet{Entity}JsonAsync` returning `DataTableResponseDto<T>`.

**AJAX forms:** Use `data-ajax="true"` with `asp-page-handler="Create"` and `Success_Create` callback. Create/Edit pages render inside `#ContainerModal` div via AJAX. Use `_DetailsActions.cshtml` partials for entity-specific action buttons.

**Results:** `OperationResult<T>` with `.Success()`, `.Info()`, `.Error()`.
**Grid filter:** `FilterGrd` with `Search`, `Page`, `PageSize`, `SortBy`, `SortDesc`.

### Pedido — Estado y Eventos (Ventas)

**Enums** (en `Matvi.Back.Application/Common/Enums/`):

- `EstatusPedidoEnm` — estatus de **header** (`OpPedido.IdCtEstatusPedido`):
  `CREADO=1, APARTADO=2, EN_PROCESO=3, ENTREGADO=4, ENTREGADO_PARCIAL=5, RECHAZADO=6, CANCELADO=7`
- `EstatusPedidoDetalleEnm` — estatus de **línea** (`OpPedidoDetalle.IdCtEstatusPedidoDetalle`):
  `PENDIENTE=1, EN_PROCESO=2, CERRADO=3, CANCELADO=4`
- `TipoEventoPedidoDetalleEnm` — tipo de evento en `OpPedidoDetalleEvento`:
  `RESERVADO=1, SURTIDO=2, EN_REPARTO=3, ENTREGADO=4, RECHAZADO=5, CANCELADO=6`

**Event sourcing (OpPedidoDetalle).** Cada cambio material sobre una línea se persiste como fila en `OpPedidoDetalleEvento`. Los totales agregados (`CantidadSurtida`, `CantidadEntregada`, `CantidadRechazada`, `CantidadCancelada`) son **cache columns** para query, no source-of-truth. La fuente es la tabla de eventos; el handler `RegistrarEventoPedidoCmdHdl` los actualiza dentro de la misma transacción.

**Pending** de una línea = `Cantidad - (Surtida + Entregada + Rechazada + Cancelada)`. Validar antes de aceptar nuevos eventos.

**Máquina de estados del header** (`AjustarEstatusPedidoCmdHdl`):
```
CREADO       → APARTADO | EN_PROCESO | CANCELADO
APARTADO     → EN_PROCESO | CANCELADO
EN_PROCESO   → ENTREGADO | ENTREGADO_PARCIAL | RECHAZADO | CANCELADO
ENTREGADO | ENTREGADO_PARCIAL | RECHAZADO | CANCELADO  → terminales
```
Transiciones no permitidas devuelven `OperationResult.Info(false, ...)`. No-op si el estatus ya coincide (evita duplicar filas en `OpPedidoEstatus`).

**Apartado.** `CrearPedidoDto.EsApartadoFld=true` ⇒ pedido se crea `APARTADO`, sin tocar inventario (stock se valida pero no se mueve). El surtido/reserva se hace después con `RegistrarEventoPedidoCmd(TipoEvento=RESERVADO)` cuando el cliente retira.

**Captura (`CrearPedidoCmdHdl`)** invoca `ValidarCapturaPedidoQry` antes de insertar. Reglas encapsuladas (Cliente 1-10, Vendedor 3-5): cliente mostrador solo con canal presencial; pago = total cuando es mostrador; apartado no permite mostrador; etc. Ver `ValidarCapturaPedidoQry.cs`.

### Code Generation (PowerShell)
The two root-level `.ps1` scripts regenerate persistence code from the SQL schema — do not edit generated `Ett`/`Cfg` files by hand:
```powershell
.\1 Generar-Tbl.ps1   # Generate entity classes (Ett) from DB schema
.\2 Generar-Cfg.ps1   # Generate EF configurations (Cfg)
```
Output: `Matvi.Back.Infrastructure/Persistence/Entities/` and `Configurations/`.

**NOT NULL columns without DEFAULT** (e.g. `OpPedidoDetalle.CantidadSurtida`, `CantidadCancelada`): every insert must pass them explicitly. Treat as **required** in DTO/handler code — do not rely on EF default.

## Conventions

- **Entities:** suffix `Ett` (e.g., `OpCompraEtt`, `CtProductoEtt`)
- **DTOs:** suffix `Dto` (e.g., `CrearCompraDto`, `ListarProductoDto`)
- **Commands:** suffix `Cmd` + handler `CmdHdl` (e.g., `CrearCompraCmd`, `CrearCompraCmdHdl`)
- **Queries:** suffix `Qry` + handler `QryHdl` (e.g., `ListarCompraQry`, `ListarCompraQryHdl`)
- **Navigation properties:** `Nav_{FkColumn}` (many-to-one), `Coll_{ChildTable}_{FkColumn}` (one-to-many)

## Frontend Libraries
- Select2 (dropdowns) via `Matvi.initSelect2()`
- jQuery BlockUI for loading states
- DataTables for grids (config in `matvi.events.js`)
- Custom stepper: `matvi.stepper.js` / `matvi.stepper.css`

## Database
- Server: `LOCALHOST\SQLEXPRESS` | Database: `MatviDB`
- Table prefixes: `Ct*` catalogs, `Op*` operations, `Rl*` relationships, `Bt*` audit
- Consecutive numbering via `BtConsecutivo` table
- Schema scripts: `CRG_Matvi.sql` (catalog), `EST_Matvi.sql` (structure)
