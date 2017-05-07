using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace PeoplesSource.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private IPersistence _persistence;

        public Repository(IPersistence persistence)
        {
            _persistence = persistence.ThrowIfNull("persistence");
        }

        public ISession Session { get { return _persistence.Session; } }

        public TEntity GetById(int id)
        {
            TEntity entity = null;

            entity = Session.Get<TEntity>(id);

            return entity;
        }

        public TEntity GetById(Guid id)
        {
            TEntity entity = null;

            entity = Session.Get<TEntity>(id);

            return entity;
        }

        public IList<TEntity> GetAll()
        {
            IList<TEntity> entities = null;

            entities = Session.Query<TEntity>().ToList();

            return entities;
        }

        public IList<TEntity> GetAllWithFetch(params Expression<Func<TEntity, object>>[] fetchExpressions)
        {
            var query = Session.Query<TEntity>();

            if (fetchExpressions != null && fetchExpressions.Any())
            {
                var queryWithFetches = query.Fetch(fetchExpressions.First());
                foreach (var fetchExpression in fetchExpressions.Skip(1))
                {
                    queryWithFetches = queryWithFetches.Fetch(fetchExpression);
                }
                return queryWithFetches.ToList();
            }

            return query.ToList();
        }

        public void Save(TEntity entity)
        {
            if (!Session.Contains(entity))
            {
                Session.SaveOrUpdate(entity);
            }
            Session.Flush();
        }

        public void Delete(int id)
        {
            var productToDelete = Session.Load<TEntity>(id);
            Session.Delete(productToDelete);
            Session.Flush();
        }

        public void Delete(TEntity entityToDelete)
        {
            Session.Delete(entityToDelete);
            Session.Flush();
        }

        public TEntity GetProxy(int id)
        {
            return Session.Load<TEntity>(id);
        }

        public IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>> where)
        {
            return Session.Query<TEntity>().Where(where).ToList();
        }

        public IList<TEntity> GetAllByWithFetch(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] fetchExpressions)
        {
            var query = Session.Query<TEntity>().Where(where);

            if (fetchExpressions != null && fetchExpressions.Any())
            {
                var queryWithFetches = query.Fetch(fetchExpressions.First());
                foreach (var fetchExpression in fetchExpressions.Skip(1))
                {
                    queryWithFetches = queryWithFetches.Fetch(fetchExpression);
                }
                return queryWithFetches.ToList();
            }

            return query.ToList();
        }

        public IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>> where, int take)
        {
            return Session.Query<TEntity>().Where(where).Take(take).ToList();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> where)
        {
            return Session.Query<TEntity>().Where(where).FirstOrDefault();
        }

        public int CountAllBy(Expression<Func<TEntity, bool>> where)
        {
            return Session.Query<TEntity>().Count(where);
        }

        public TEntity New()
        {
            return new TEntity();
        }

        public IQueryable<TEntity> Query()
        {
            return Session.Query<TEntity>();
        }
    }
}
