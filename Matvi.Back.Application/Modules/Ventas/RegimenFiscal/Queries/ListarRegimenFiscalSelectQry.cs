using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Queries
{
    public record ListarRegimenFiscalSelectQry()
        : IRequest<List<BaseDto<short>>>;
    
}
