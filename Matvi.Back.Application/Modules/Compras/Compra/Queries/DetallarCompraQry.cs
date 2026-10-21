using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
     public record DetallarCompraQry(int id)
        :IRequest<DetallarCompraDto>;

}
