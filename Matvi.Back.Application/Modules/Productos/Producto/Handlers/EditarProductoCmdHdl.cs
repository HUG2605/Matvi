using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Commands;
using Matvi.Back.Application.Modules.Productos.Producto.Helpers;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    [Transaction]
    class EditarProductoCmdHdl(
        IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarProductoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EditarProductoCmd request,
            CancellationToken cancellationToken)
        {
            var productoDto = request.producto;

            #region ACTUALIZAR PRODUCTO

            var productoEtt = await _unitOfWork.FindAsync<CtProductoEtt, int>(productoDto.Id, cancellationToken);

            productoEtt!.Nombre = productoDto.NombreFld;
            productoEtt.EspecificacionTecnica = productoDto.EspecificacionTecnicaFld;
            productoEtt.Marca = productoDto.MarcaFld;
            productoEtt.IdCtGrupo = productoDto.CtGrup_IdFld;
            productoEtt.BajoPedido = productoDto.BajoPedidoFld;
            productoEtt.EntregaMostrador = productoDto.EntregaMostradorFld;
            productoEtt.Imagen = string.IsNullOrEmpty(productoDto.ImagenFld) == false 
                ? productoDto.ImagenFld
                : productoEtt.Imagen;
            productoEtt.CodigoBarrasInterno = productoDto.CodigoBarrasInternoFld;
            productoEtt.CodigoBarras = productoDto.CodigoBarrasInternoFld
                ? ProductoQueriesHelper.GenerarCodigoInterno(productoDto.Id)
                : productoDto.CodigoBarrasFld;
            productoEtt.Activo = productoDto.ActivoFld;

            _unitOfWork.Update(productoEtt);

            #endregion

            #region ELIMINAR RELACIONES EXISTENTES CON PROVEEDORES

            var relaciones = await _unitOfWork.Query<RlProductoProveedorEtt>()
                .Where(i => i.IdCtProducto == productoDto.Id)
                .ToListAsync(cancellationToken);

            _unitOfWork.RemoveRange(relaciones);

            #endregion

            #region AGREGAR RELACIONES NUEVAS CON PROVEEDORES

            var lstRelProProEtt = productoDto.Proveedores?
                .Where(p => p.EstaRelacionado)
                .Select(p => new RlProductoProveedorEtt
                {
                    IdCtProducto = productoDto.Id,
                    IdCtProveedor = p.CtProv_Id
                }).ToList();

            if (lstRelProProEtt is { Count: > 0 })
            {
                _unitOfWork.AddRange(lstRelProProEtt);
            }

            #endregion 

            return OperationResult<bool>.Success(
                true, 
                "El producto se editó correctamente.");
        }
    }
}
