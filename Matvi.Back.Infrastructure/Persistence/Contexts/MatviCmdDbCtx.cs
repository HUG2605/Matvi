using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Infrastructure.Persistence.Contexts
{
    public partial class MatviCmdDbCtx : DbContext
    {
        public MatviCmdDbCtx(DbContextOptions<MatviCmdDbCtx> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatviCmdDbCtx).Assembly);
        }
    }
}
