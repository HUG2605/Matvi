using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// Contexto de base de datos para operaciones de escritura y comandos (CUD) con soporte para auditoría.
    /// </summary>
    public partial class MatviCmdDbCtx : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MatviCmdDbCtx"/> con las opciones de configuración especificadas.
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto de base de datos.</param>
        public MatviCmdDbCtx(DbContextOptions<MatviCmdDbCtx> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatviCmdDbCtx).Assembly);
        }
    }
}
