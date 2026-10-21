using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class BtCambioEtt : IAuditable
{
    public int Id { get; set; }
    public string Tabla { get; set; } = null!;
    public string LlavePrimaria { get; set; } = null!;
    public string Campo { get; set; } = null!;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public byte TipoOperacion { get; set; }
    public short IdCtUsuario { get; set; }
    public DateTime Fecha { get; set; }
}