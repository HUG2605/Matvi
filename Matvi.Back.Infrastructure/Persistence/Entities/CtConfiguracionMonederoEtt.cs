using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtConfiguracionMonederoEtt : IAuditable
{
    public int Id { get; set; }
    public decimal PorcentajeAcumulacion { get; set; }
    public decimal CostoPremio { get; set; }
    public string NombrePremio { get; set; } = null!;
    public bool Activo { get; set; }
}