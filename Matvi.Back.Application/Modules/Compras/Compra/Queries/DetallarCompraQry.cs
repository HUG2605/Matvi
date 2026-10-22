using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
          /// <summary>
     /// Consulta CQRS para obtener información de detallar compra.
     /// </summary>
public record DetallarCompraQry(int id)
        :IRequest<DetallarCompraDto>;

}
