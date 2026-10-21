using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class DetallarPedidoEstatusItemDto
    {
        public string ProductoNombre { get; set; } = string.Empty;

        public string CtProd_Nombre
        {
            get => ProductoNombre;
            set => ProductoNombre = value;
        }

        public string PresentacionNombre { get; set; } = string.Empty;

        public string CtPresProd_Nombre
        {
            get => PresentacionNombre;
            set => PresentacionNombre = value;
        }

        public decimal Cantidad { get; set; }
    }

    public class DetallarPedidoEstatusDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Estatus General de Pedido

        public byte? IdCtEstatusPedido { get; set; }

        public byte? CtEstaPedi_Id
        {
            get => IdCtEstatusPedido;
            set => IdCtEstatusPedido = value;
        }

        public string CtEstatus_Nombre { get; set; } = string.Empty;

        public string CtEstaPedi_Nombre
        {
            get => CtEstatus_Nombre;
            set => CtEstatus_Nombre = value;
        }

        #endregion

        #region Evento de Detalle de Linea

        public bool EsEventoDetalle { get; set; }
        public byte? IdCtTipoEventoPedidoDetalle { get; set; }
        public string? CtTipoEvento_Nombre { get; set; }
        public List<DetallarPedidoEstatusItemDto> Productos { get; set; } = [];

        public int CantidadLineas { get; set; }

        #endregion

        #region Auditoria y Notas

        public DateTime Fecha { get; set; }
        public string CtUsuario_Nombre { get; set; } = string.Empty;
        public string? Notas { get; set; }

        #endregion

        #region Sub-Eventos Operativos

        public List<DetallarPedidoEstatusDto> SubEventos { get; set; } = [];

        #endregion
    }
}
