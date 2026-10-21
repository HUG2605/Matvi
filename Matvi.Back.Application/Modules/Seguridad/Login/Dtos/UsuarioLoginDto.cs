namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
    public class UsuarioLoginDto
    {
        public short Id { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public byte? RolId { get; set; }
        public string? RolNombre { get; set; }
        public bool Activo { get; set; }
        public List<FuncionLoginDto>? Funciones { get; set; }
        public List<AccionLoginDto>? Acciones { get; set; }
        public List<SucursalLoginDto>? Sucursales { get; set; }
        public SucursalLoginDto? SucursalSesion { get; set; }

    }


}
