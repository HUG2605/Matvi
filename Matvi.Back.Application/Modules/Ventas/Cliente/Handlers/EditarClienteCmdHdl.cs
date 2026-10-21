
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    [Transaction]
    class EditarClienteCmdHdl(
       IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarClienteCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarClienteCmd request, 
            CancellationToken cancellationToken)
        {
            var clienteDto = request.cliente;

            var clienteEtt = await _unitOfWork.FindAsync<CtClienteEtt, int>(clienteDto.Id, cancellationToken);
            clienteEtt!.Nombre = clienteDto.NombreFld;
            clienteEtt.Alias = clienteDto.AliasFld;
            clienteEtt.Direccion = clienteDto.DireccionFld;
            clienteEtt.Telefono = clienteDto.TelefonoFld;
            clienteEtt.IdCtRegimenFiscal= clienteDto.IdCatRegimenFiscalFld;
            clienteEtt.RFCFiscal = clienteDto.RFCFld;
            clienteEtt.NumeroExteriorFiscal = clienteDto.NumeroExteriorFiscalFld;
            clienteEtt.NumeroInteriorFiscal = clienteDto.NumeroInteriorFiscalFld;
            clienteEtt.CodigoPostalFiscal = clienteDto.CodigoPostalFiscalFld;
            clienteEtt.ColoniaFiscal = clienteDto.ColoniaFiscalFld;
            clienteEtt.LocalidadFiscal= clienteDto.LocalidadFiscalFld;
            clienteEtt.MunicipioFiscal = clienteDto.MunicipioFiscalFld;
            clienteEtt.EntidadFederativaFiscal = clienteDto.EntidadFiscalFld;
            clienteEtt.LimiteCreditoSemanal = clienteDto.LimiteCreditoSemanalFld;

            _unitOfWork.Update(clienteEtt);

            return OperationResult<bool>.Success(
                true, 
                "El cliente se editó correctamente.");
        }
    }
}
