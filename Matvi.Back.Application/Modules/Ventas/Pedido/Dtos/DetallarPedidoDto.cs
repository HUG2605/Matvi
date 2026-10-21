using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class DetallarPedidoDto : IIdDto<int>, IFolioDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.PEDIDO);

        #endregion

        #region Generales

        public string CtSucu_Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

        public int? IdCtCliente { get; set; }
        public int? CtClie_Id
        {
            get => IdCtCliente;
            set => IdCtCliente = value;
        }

        public string? CtClie_Nombre { get; set; }

        public string? NombreClienteMostrador { get; set; }
        public string? ClienteMostradorNombre
        {
            get => NombreClienteMostrador;
            set => NombreClienteMostrador = value;
        }

        public string? TelefonoClienteMostrador { get; set; }
        public string? ClienteMostradorTelefono
        {
            get => TelefonoClienteMostrador;
            set => TelefonoClienteMostrador = value;
        }

        public string CtCanaCapt_Nombre { get; set; } = string.Empty;

        public byte IdCtEstatusPedido { get; set; }
        public byte CtEstaPedi_Id
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

        public bool EntregaDomicilio { get; set; }
        public string? DireccionEntrega { get; set; }
        public string? Notas { get; set; }

        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal SaldoPendiente => Total - TotalPagado;

        #endregion

        #region Productos

        public List<DetallarPedidoDetalleDto> Productos { get; set; } = [];

        #endregion

        #region Cobros

        public List<DetallarPedidoCobroDto> Cobros { get; set; } = [];

        #endregion

        #region Lotes

        public List<DetallarPedidoLoteDto> Lotes { get; set; } = [];

        #endregion

        #region Historial de Estatus

        public List<DetallarPedidoEstatusDto> HistorialEstatus { get; set; } = [];

        #endregion
    }
}