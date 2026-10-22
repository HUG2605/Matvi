using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar corte caja.
    /// </summary>
public record DetallarCorteCajaQry(int Id)
        : IRequest<DetallarCorteCajaDto?>;
}