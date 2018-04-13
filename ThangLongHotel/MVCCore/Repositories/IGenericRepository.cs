using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;

namespace MVCCore.Repositories
{
    public interface IGenericRepository<TEntity> : IBaseRepository
        where TEntity : class
    {
        DbContextTransaction BeginTransaction();

        IQueryable<TEntity> GetAll();
        TEntity GetByID(int id);



        TEntity GetEntity(params Expression<Func<TEntity, object>>[] includes);
        TEntity GetEntity(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes);
        TEntity GetEntity(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        TEntity GetEntity(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);


        ICollection<TEntity> GetEntities(params Expression<Func<TEntity, object>>[] includes);
        ICollection<TEntity> GetEntities(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes);
        ICollection<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        ICollection<TEntity> GetEntities(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);




        TEntity Add(TEntity entity);
        TEntity Remove(TEntity entity);

        int SaveChanges();

    }
}
