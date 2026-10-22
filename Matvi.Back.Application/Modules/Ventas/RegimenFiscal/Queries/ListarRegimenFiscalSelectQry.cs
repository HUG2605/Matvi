using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar regimen fiscal select.
    /// </summary>
public record ListarRegimenFiscalSelectQry()
        : IRequest<List<BaseDto<short>>>;
    
}
