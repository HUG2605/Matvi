using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Commands;
using Matvi.Back.Application.Modules.Productos.Producto.Helpers;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    [Transaction]
    class CrearProductoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearProductoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            CrearProductoCmd request,
            CancellationToken cancellationToken)
        {
            var productoDto = request.producto;

            #region AGREGAR NUEVO PRODUCTO

            var nvoProductoEtt = new CtProductoEtt
            {
                IdCtGrupo = productoDto.CtGrup_IdFld,
                Nombre = productoDto.NombreFld,
                EspecificacionTecnica = productoDto.EspecificacionTecnicaFld,
                Marca = productoDto.MarcaFld,
                IdCtUnidadBaseInventario = productoDto.CtUnidBaseInve_IdFld,
                BajoPedido = productoDto.BajoPedidoFld,
                Imagen = productoDto.ImagenFld,
                CodigoBarrasInterno = productoDto.CodigoBarrasInternoFld,
                CodigoBarras = productoDto.CodigoBarrasFld,
                Activo = productoDto.ActivoFld
            };

            _unitOfWork.Add(nvoProductoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region ACTUALIZAR CÓDIGO DE BARRAS INTERNO

            if (productoDto.CodigoBarrasInternoFld)
            {
                nvoProductoEtt.CodigoBarras = ProductoQueriesHelper.GenerarCodigoInterno(nvoProductoEtt.Id);
                _unitOfWork.Update(nvoProductoEtt);
            }

            #endregion

            #region AGREGAR RELACIONES NUEVAS CON PROVEEDORES

            var lstRelProProEtt = productoDto.Proveedores?
                .Where(p => p.EstaRelacionado)
                .Select(p => new RlProductoProveedorEtt
                {
                    IdCtProducto = nvoProductoEtt.Id,
                    IdCtProveedor = p.CtProv_Id
                }).ToList();

            if (lstRelProProEtt is { Count: > 0 })
            {
                _unitOfWork.AddRange(lstRelProProEtt);
            }

            #endregion

            #region CREAR PRESENTACION PRODUCTO BASE

            var nvaPresProdBaseEtt = new CtPresentacionProductoEtt
            {
                IdCtProducto = nvoProductoEtt.Id,
                Nombre = _unitOfWork.Query<CtUnidadBaseInventarioEtt>()
                    .First(i => i.Id == productoDto.CtUnidBaseInve_IdFld).Nombre,
                FactorUBI = 1,
                EsParaCompra = false,
                EsParaVenta = false,
                PrecioVenta = 0,
                PrecioMayoreo = null,
                UnidadesMayoreo = null
            };

            _unitOfWork.Add(nvaPresProdBaseEtt);

            #endregion

            #region RELACIONAR PRODUCTO CON TODAS LAS SUCURSALES

            var lstIdSucursales = _unitOfWork.Query<CtSucursalEtt>()
                .Select(s => s.Id)
                .ToList();

            var lstRelProductoSucursalEtt = lstIdSucursales
                .Select(idSucursal => new RlProductoSucursalEtt
                {
                    IdCtProducto = nvoProductoEtt.Id,
                    IdCtSucursal = idSucursal,
                    Regularizado = false,
                    Ubicacion = null
                }).ToList();

            if (lstRelProductoSucursalEtt is { Count: > 0 })
            {
                _unitOfWork.AddRange(lstRelProductoSucursalEtt);
            }

            #endregion

            return OperationResult<bool>.Success(
                true,
                "El producto se creó correctamente.");
        }
    }
}
