using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos
{
    public class ListarCobroDto: IIdDto<int>, IFolioDto
    {

        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion


        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.COBRO);

        #endregion

        public DateTime Fecha { get; set; }
        public string CtClie_Nombre { get; set; }=string.Empty;
        public decimal MontoTotal { get; set; }
        public string CtMediPago_Nombre { get; set; } = string.Empty;
        public string CtUsuaReci_Nombre { get; set; } = string.Empty;
    }
}
