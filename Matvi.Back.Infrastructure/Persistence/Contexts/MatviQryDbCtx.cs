using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// Contexto de base de datos optimizado para operaciones de lectura y consultas (R) sin interceptores de auditoría.
    /// </summary>
    public partial class MatviQryDbCtx : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MatviQryDbCtx"/> con las opciones de configuración especificadas.
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto de base de datos.</param>
        public MatviQryDbCtx(DbContextOptions<MatviQryDbCtx> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatviQryDbCtx).Assembly);
        }
    }
}
