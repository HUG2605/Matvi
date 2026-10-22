using Microsoft.EntityFrameworkCore.Storage;

namespace Matvi.Back.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Patrón Unidad de Trabajo (Unit of Work) para coordinar transacciones y operaciones CRUD sobre el contexto de comandos.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Transacciones

        /// <summary>
        /// Inicia una nueva transacción asíncrona en el contexto de base de datos si no existe una activa.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>La transacción de base de datos activa.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Confirma los cambios realizados dentro de la transacción activa y la libera.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task CommitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Revierte los cambios realizados dentro de la transacción activa y la libera.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task RollbackAsync(CancellationToken cancellationToken);

        #endregion

        #region Persistencia

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto de base de datos e invoca la auditoría con el usuario actual.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        #endregion

        #region CRUD genérico

        /// <summary>
        /// Agrega una nueva entidad al contexto para su inserción.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <param name="entity">Instancia de la entidad a agregar.</param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Agrega un conjunto de entidades al contexto para su inserción masiva.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <param name="entities">Colección de entidades a agregar.</param>
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// Marca una entidad como modificada para su actualización en la base de datos.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <param name="entity">Instancia de la entidad a modificar.</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Marca una entidad para su eliminación en la base de datos.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <param name="entity">Instancia de la entidad a eliminar.</param>
        void Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Marca un conjunto de entidades para su eliminación en la base de datos.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <param name="entities">Colección de entidades a eliminar.</param>
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// Busca una entidad por su clave primaria de forma asíncrona.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <typeparam name="TId">Tipo de la clave primaria.</typeparam>
        /// <param name="id">Valor de la clave primaria.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>La entidad encontrada o <c>null</c> si no existe.</returns>
        Task<TEntity?> FindAsync<TEntity, TId>(TId id, CancellationToken cancellationToken) where TEntity : class;
        
        /// <summary>
        /// Proporciona acceso a una consulta <see cref="IQueryable{TEntity}"/> sobre el conjunto de entidades.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
        /// <returns>Consulta consultable para el conjunto de entidades.</returns>
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        #endregion 

        #region Métodos para adjuntar padres automáticamente

        /// <summary>
        /// Adjunta las entidades padre relacionadas en estado sin cambios para evitar duplicidades al insertar dependencias.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad hija.</typeparam>
        /// <param name="entities">Colección de entidades hijas cuyos padres deben adjuntarse.</param>
        void AttachParentsRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        #endregion 

    }
}