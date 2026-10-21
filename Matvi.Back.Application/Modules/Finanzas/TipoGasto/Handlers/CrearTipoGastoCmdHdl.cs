using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    [Transaction]
    class CrearTipoGastoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearTipoGastoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            CrearTipoGastoCmd request, 
            CancellationToken cancellationToken)
        {
            var nvoTipoGastoEtt = new CtTipoGastoEtt
            {
                Nombre = request.tipoGasto.NombreFld
            };
            _unitOfWork.Add(nvoTipoGastoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El tipo de gasto se creó correctamente.");
        }
    }
}
