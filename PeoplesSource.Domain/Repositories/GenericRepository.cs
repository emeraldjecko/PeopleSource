using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace PeoplesSource.Domain.Repositories
{
    /// <summary>
    /// Class GenericRepository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// The _session
        /// </summary>
        private ISession _session = SessionManager.CurrentSession;


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IList{`0}.</returns>
        public IList<TEntity> GetAll()
        {
            return this._session.Query<TEntity>().ToList();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IList{`0}.</returns>
        public IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter)
        {
            return this._session.Query<TEntity>()
                .Where(filter).ToList();
        }

        /// <summary>
        /// Withes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>`0.</returns>
        public TEntity With(int id)
        {
            return this._session.Get<TEntity>(id);
        }

        /// <summary>
        /// Persists the specified item to persist.
        /// </summary>
        /// <param name="itemToPersist">The item to persist.</param>
        /// <returns>`0.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The expectation here is that the item persisted (whether new or updated) should now have a valid Id property for the caller to then use. ^MK
        /// or
        /// I'm not sure why we can't SaveOrUpdate if the item is in the session so I'm throwing to highlight this case. ^MK
        /// </exception>
        public TEntity Persist(TEntity itemToPersist)
        {
            if (!this._session.Contains(itemToPersist))
            {
                this._session.SaveOrUpdate(itemToPersist);
                this._session.Flush();

                if (itemToPersist.Id < 1) throw new InvalidOperationException("The expectation here is that the item persisted (whether new or updated) should now have a valid Id property for the caller to then use. ^MK");

                return itemToPersist;
            }
            else throw new InvalidOperationException("I'm not sure why we can't SaveOrUpdate if the item is in the session so I'm throwing to highlight this case. ^MK");
        }

        /// <summary>
        /// Deletes the specified item to delete.
        /// </summary>
        /// <param name="itemToDelete">The item to delete.</param>
        public void Delete(TEntity itemToDelete)
        {
            this._session.Delete(itemToDelete);
            this._session.Flush();
        }
    }
}
