using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using MVCModel.Models;
using MVCCore.Repositories;

namespace MVCData.Repositories
{
    public class GenericWithDetailRepository<TEntity, TEntityDetail> : GenericRepository<TEntity>, IGenericWithDetailRepository<TEntity, TEntityDetail>
        where TEntity : class
        where TEntityDetail : class
    {
        private readonly HotelManagerEntities totalBikePortalsEntities;
        private DbSet<TEntityDetail> modelDetailDbSet = null;

        public GenericWithDetailRepository(HotelManagerEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
            modelDetailDbSet = this.totalBikePortalsEntities.Set<TEntityDetail>();
        }


        public TEntityDetail RemoveDetail(TEntityDetail entityDetail)
        {
            return this.modelDetailDbSet.Remove(entityDetail);
        }

        public IEnumerable<TEntityDetail> RemoveRangeDetail(IEnumerable<TEntityDetail> entityDetails)
        {
            return this.modelDetailDbSet.RemoveRange(entityDetails);
        }
    }
}
