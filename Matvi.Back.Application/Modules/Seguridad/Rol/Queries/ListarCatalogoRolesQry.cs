using Matvi.Back.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar catalogo roles.
    /// </summary>
public record ListarCatalogoRolesQry 
        : IRequest<List<BaseDto<byte>>>;
}
