using Microsoft.EntityFrameworkCore.Storage;

namespace Matvi.Back.Infrastructure.Persistence.Repositories
{
    public interface IUnitOfWork
    {
        #region Transacciones

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);

        #endregion

        #region Persistencia

        Task SaveChangesAsync(CancellationToken cancellationToken);

        #endregion

        #region CRUD genérico

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;

        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        Task<TEntity?> FindAsync<TEntity, TId>(TId id, CancellationToken cancellationToken) where TEntity : class;
        
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        #endregion 

        #region Métodos para adjuntar padres automáticamente

        void AttachParentsRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        #endregion 

    }
}