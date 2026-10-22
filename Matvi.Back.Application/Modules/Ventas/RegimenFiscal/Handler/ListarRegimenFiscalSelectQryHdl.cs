using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Queries;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Handler
{
    class ListarRegimenFiscalSelectQryHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<ListarRegimenFiscalSelectQry, List<BaseDto<short>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<short>>> Handle(ListarRegimenFiscalSelectQry request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Query<CtRegimenFiscalEtt>()
            .Select(crf => new BaseDto<short>
            {
                Id = crf.Id,
                Nombre = crf.Nombre,
            });
            
            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
