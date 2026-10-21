using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Configurations;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos
{
    public class ListarGastoDto: IIdDto<int>, IFolioDto
    {

        #region Implementación de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.GASTO);

        #endregion
        public DateTime FechaPago { get; set; }
        public string CtTipoGast_Nombre { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public string CtMedioPago_Nombre { get; set; } = string.Empty;
        public string CtUsua_Nombre { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;

    }
}
