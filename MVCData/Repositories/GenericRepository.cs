using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Data.Entity;

using MVCModel.Models;
using MVCCore.Repositories;

namespace MVCData.Repositories
{
    public class GenericRepository<TEntity> : BaseRepository, IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly HotelManagerEntities totalBikePortalsEntities;
        private DbSet<TEntity> modelDbSet = null;

        public GenericRepository(HotelManagerEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
            modelDbSet = this.totalBikePortalsEntities.Set<TEntity>();
        }



        public DbContextTransaction BeginTransaction()
        {
            return this.totalBikePortalsEntities.Database.BeginTransaction();
        }



        public IQueryable<TEntity> GetAll()
        {
            return this.modelDbSet;
        }

        public TEntity GetByID(int id)
        {
            return this.modelDbSet.Find(id);
        }



        public TEntity GetEntity(params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(includes);
        }
        public TEntity GetEntity(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(proxyCreationEnabled, includes);
        }
        public TEntity GetEntity(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(predicate, includes);
        }
        public TEntity GetEntity(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(proxyCreationEnabled, predicate, includes);
        }




        public ICollection<TEntity> GetEntities(params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(includes);
        }
        public ICollection<TEntity> GetEntities(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(proxyCreationEnabled, includes);
        }
        public ICollection<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(predicate, includes);
        }
        public ICollection<TEntity> GetEntities(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(proxyCreationEnabled, predicate, includes);
        }





        public TEntity Add(TEntity entity)
        {
            return this.modelDbSet.Add(entity);
        }

        public TEntity Remove(TEntity entity)
        {
            return this.modelDbSet.Remove(entity);
        }

        public int SaveChanges()
        {
            return this.totalBikePortalsEntities.SaveChanges();
        }


    }
}
