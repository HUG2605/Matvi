using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar rol.
    /// </summary>
public record ListarRolQry(): IRequest<List<ListarRolDto>>;
}
