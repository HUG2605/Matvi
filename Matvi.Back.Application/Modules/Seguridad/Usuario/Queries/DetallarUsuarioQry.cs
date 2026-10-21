using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
    public record DetallarUsuarioQry(
        short id
        ) : IRequest<DetallarUsuarioDto>;
}
