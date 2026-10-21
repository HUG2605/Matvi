using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
    public class RolAccionAccesoPermisoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public int FuncionId { get; set; }
        public string? AccionNombre { get; set; }
        public bool TieneAcceso { get; set; }
    }
}
