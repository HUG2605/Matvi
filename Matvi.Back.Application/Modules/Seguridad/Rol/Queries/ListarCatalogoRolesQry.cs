using Matvi.Back.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
    public record ListarCatalogoRolesQry 
        : IRequest<List<BaseDto<byte>>>;
}
