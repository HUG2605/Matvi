# Matvi - Agent Guidelines

## Build & Run
```bash
dotnet build Matvi.sln
dotnet run --project Matvi.Front.UI
```
Dev server: `http://localhost:5031` | `https://localhost:7022`

## Architecture

**ASP.NET Core 10.0 Razor Pages** with Areas + CQRS (MediatR) + EF Core (SQL Server)

### Project Structure
```
Matvi.Front.UI/          → Web layer (Razor Pages, Controllers, JS)
Matvi.Back.Application/  → Business logic (Commands, Queries, Handlers, DTOs)
Matvi.Back.Infrastructure/ → Data access (EF Core, Repositories, Interceptors)
Matvi.Transversal/       → Shared exceptions
```

### Module Organization
Each module (Compras, Configuracion, Finanzas, Inventarios, Productos, Seguridad, Ventas) follows:
- **Frontend**: `Areas/{Module}/Pages/{Entity}/` (CRUD pages) + `Areas/{Module}/Api/` (Controllers)
- **Backend**: `Modules/{Module}/{Entity}/` with `Commands/`, `Queries/`, `Handlers/`, `Dtos/`
- **JS**: `wwwroot/js/Modules/{Module}/{Entity}/` (e.g., `Create.js`, `Index.js`)

### Key Patterns

**Dual DbContext:**
- `MatviCmdDbCtx` (scoped, with `AuditInterceptor`) → Commands
- `MatviQryDbCtx` (DbContextFactory, no interceptors) → Queries

**Authorization:** `[AuthorizeMatvi(Funcion.X, Accion.Y)]` attribute on PageModels
- `Funcion.cs` → Module permissions (e.g., `Compras_Compras`, `Ventas_Pedidos`)
- `Accion.cs` → Actions (Crear, Editar, Eliminar, Surtir, Regularizar)

**PageModel Convention:** Inherit `BaseModel` (not `PageModel`) for obfuscation helpers

**Grid Pattern:** DataTables with `OnGet{Entity}JsonAsync` returning `DataTableResponseDto<T>`

**AJAX Forms:** Use `data-ajax="true"` with `asp-page-handler="Create"` and `Success_Create` callback

**Modals:** Create/Edit pages render inside `#ContainerModal` div via AJAX

**Partial Views:** `_DetailsActions.cshtml` for entity-specific action buttons

### Code Generation (PowerShell)
```powershell
.\1 Generar-Tbl.ps1   # Generate entity classes (Ett) from DB schema
.\2 Generar-Cfg.ps1    # Generate EF configurations (Cfg)
```
Output: `Matvi.Back.Infrastructure/Persistence/Entities/` and `Configurations/`

## Conventions

- **Entities:** Suffix `Ett` (e.g., `OpCompraEtt`, `CtProductoEtt`)
- **DTOs:** Suffix `Dto` (e.g., `CrearCompraDto`, `ListarProductoDto`)
- **Commands:** Suffix `Cmd` + Handler `CmdHdl` (e.g., `CrearCompraCmd`, `CrearCompraCmdHdl`)
- **Queries:** Suffix `Qry` + Handler `QryHdl` (e.g., `ListarCompraQry`, `ListarCompraQryHdl`)
- **Navigation:** `Nav_{FkColumn}` (many-to-one), `Coll_{ChildTable}_{FkColumn}` (one-to-many)
- **Results:** `OperationResult<T>` with `.Success()`, `.Info()`, `.Error()`
- **Grid Filter:** `FilterGrd` with Search, Page, PageSize, SortBy, SortDesc

## Frontend Libraries
- Select2 (dropdowns) via `Matvi.initSelect2()`
- jQuery BlockUI for loading states
- DataTables for grids (config in `matvi.events.js`)
- Custom stepper: `matvi.stepper.js` / `matvi.stepper.css`

## Database
- Server: `LOCALHOST\SQLEXPRESS` | Database: `MatviDB`
- Tables: `Ct*` (catalogs), `Op*` (operations), `Rl*` (relationships), `Bt*` (audit)
- Consecutive numbering via `BtConsecutivo` table
