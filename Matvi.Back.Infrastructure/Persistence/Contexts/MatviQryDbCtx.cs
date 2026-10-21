using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Infrastructure.Persistence.Contexts
{
    public partial class MatviQryDbCtx : DbContext
    {
        public MatviQryDbCtx(DbContextOptions<MatviQryDbCtx> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatviQryDbCtx).Assembly);
        }
    }
}
