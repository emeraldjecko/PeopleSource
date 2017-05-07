using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PeoplesSource.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(int id);
        void Delete(TEntity entityToDelete);
        IList<TEntity> GetAll();
        IList<TEntity> GetAllWithFetch(params Expression<Func<TEntity, object>>[] fetchExpressions);
        TEntity GetById(int id);
        TEntity GetById(Guid id);
        TEntity GetProxy(int id);
        void Save(TEntity entity);
        IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>> where);
        IList<TEntity> GetAllByWithFetch(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] fetchExpressions);
        IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>> where, int take);
        TEntity GetFirst(Expression<Func<TEntity, bool>> where);
        int CountAllBy(Expression<Func<TEntity, bool>> where);
        TEntity New();
        IQueryable<TEntity> Query();
    }
}
