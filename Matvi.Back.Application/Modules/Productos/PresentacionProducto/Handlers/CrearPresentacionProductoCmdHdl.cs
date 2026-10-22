using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    [Transaction]
    class CrearPresentacionProductoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearPresentacionProductoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearPresentacionProductoCmd request, 
            CancellationToken cancellationToken)
        {
            var presProdDto = request.presentacionProducto;

            #region VALIDACIONES

            #region PRECIO DE MAYOREO

            // Validación: si uno tiene valor, el otro también debe tener
            bool tienePrecioMayoreo = presProdDto.PrecioMayoreoFld.HasValue;
            bool tieneUnidadesMayoreo = presProdDto.UnidadesMayoreoFld.HasValue;

            if (tienePrecioMayoreo && !tieneUnidadesMayoreo)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe especificar la cantidad mínima para mayoreo cuando se define un precio de mayoreo.");
            }

            if (!tienePrecioMayoreo && tieneUnidadesMayoreo)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe especificar el precio de mayoreo cuando se define una cantidad mínima para mayoreo.");
            }

            if (tienePrecioMayoreo && presProdDto.PrecioMayoreoFld >= presProdDto.PrecioVentaFld)
            {
                return OperationResult<bool>.Info(
                    false,
                    "El precio de mayoreo debe ser menor al precio de venta.");
            }

            #endregion

            #endregion

            #region AGREGAR NUEVA PRESENTACION DEL PRODUCTO

            var nvoPresentacionProductoEtt = new CtPresentacionProductoEtt
            {
                IdCtProducto = presProdDto.CtProd_IdFld,
                Nombre = presProdDto.NombreFld,
                FactorUBI = presProdDto.FactorUBIFld,
                EsParaCompra = presProdDto.EsParaCompraFld,
                EsParaVenta = presProdDto.EsParaVentaFld,
                PrecioVenta = presProdDto.PrecioVentaFld,
                PrecioMayoreo = presProdDto.PrecioMayoreoFld,
                UnidadesMayoreo = presProdDto.UnidadesMayoreoFld
            };

            _unitOfWork.Add(nvoPresentacionProductoEtt);

            #endregion

            return OperationResult<bool>.Success(
                true,
                "La presentación del producto se creó correctamente.");
        }
    }
}
