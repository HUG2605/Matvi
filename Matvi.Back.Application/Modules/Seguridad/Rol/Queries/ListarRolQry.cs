using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
    public record ListarRolQry(): IRequest<List<ListarRolDto>>;
}
