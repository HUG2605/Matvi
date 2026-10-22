using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar usuario.
    /// </summary>
public record DetallarUsuarioQry(
        short id
        ) : IRequest<DetallarUsuarioDto>;
}
