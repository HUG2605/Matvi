using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    [Transaction]
    class CrearClienteCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearClienteCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearClienteCmd request, 
            CancellationToken cancellationToken)
        {
            var clienteDto = request.cliente;

            var nvoCliente = new CtClienteEtt
            {
                //Datos Generales 
                Nombre = clienteDto.NombreFld,
                Alias = clienteDto.AliasFld,
                Direccion = clienteDto.DireccionFld,
                Telefono = clienteDto.TelefonoFld,
                //Datos Fiscales 
                RFCFiscal = clienteDto.RFCFld,
                IdCtRegimenFiscal = clienteDto.IdCatRegimenFiscalFld,
                CalleFiscal = clienteDto.CalleFiscalFld,
                NumeroExteriorFiscal = clienteDto.NumeroExteriorFiscalFld,
                NumeroInteriorFiscal = clienteDto.NumeroInteriorFiscalFld,
                CodigoPostalFiscal = clienteDto.CodigoPostalFiscalFld,
                ColoniaFiscal = clienteDto.ColoniaFiscalFld,
                LocalidadFiscal = clienteDto.LocalidadFiscalFld,
                MunicipioFiscal = clienteDto.MunicipioFiscalFld,
                EntidadFederativaFiscal = clienteDto.EntidadFiscalFld,
                LimiteCreditoSemanal = clienteDto.LimiteCreditoSemanalFld
            };

            _unitOfWork.Add(nvoCliente);

            return OperationResult<bool>.Success(
                true, 
                "El cliente se creó correctamente.");
        }
    }
}
