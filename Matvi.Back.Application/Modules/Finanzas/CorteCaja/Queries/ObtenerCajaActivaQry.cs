using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries
{
    public record ObtenerCajaActivaQry(
        byte idSucursal,
        short idUsuario
    ) : IRequest<CajaActivaDto?>;
}