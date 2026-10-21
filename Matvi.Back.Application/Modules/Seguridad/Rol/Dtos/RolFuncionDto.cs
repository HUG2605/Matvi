using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
    public class RolFuncionDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public byte FuncionId { get; set; }
        public string? FuncionNombre { get; set; }
        public byte ModuloId { get; set; }
        public string? ModuloNombre { get; set; }
        public bool Consultar { get; set; }
        public bool VerSensibles { get; set; }
        public List<RolAccionAccesoPermisoDto>? Acciones { get; set; }
    }
}
