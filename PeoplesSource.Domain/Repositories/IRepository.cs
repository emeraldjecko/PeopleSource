using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PeoplesSource.Domain.Repositories
{
    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : EntityObject
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IList{`0}.</returns>
        IList<TEntity> GetAll();
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IList{`0}.</returns>
        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Withes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>`0.</returns>
        TEntity With(int id);

        /// <summary>
        /// Persists the specified item to persist.
        /// </summary>
        /// <param name="itemToPersist">The item to persist.</param>
        /// <returns>`0.</returns>
        TEntity Persist(TEntity itemToPersist);
        /// <summary>
        /// Deletes the specified item to delete.
        /// </summary>
        /// <param name="itemToDelete">The item to delete.</param>
        void Delete(TEntity itemToDelete);
    }


    /// <summary>
    /// Interface IBulkRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IBulkRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityObject
    {
        // The idea here is that not all repos are repos of "volume"
        // Some repos are safe to return all items in a single list and persist indiviauals
        // Some repos are larger and require paging and batch updates

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>IList{`0}.</returns>
        IList<TEntity> GetAll(int limit);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IList{`0}.</returns>
        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, int limit);

        /// <summary>
        /// Persists the specified items to persist.
        /// </summary>
        /// <param name="itemsToPersist">The items to persist.</param>
        /// <returns>IList{`0}.</returns>
        IList<TEntity> Persist(IList<TEntity> itemsToPersist);
        /// <summary>
        /// Deletes the specified items to delete.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        void Delete(IList<TEntity> itemsToDelete);
    }
}
