using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{    
    public class ListChargeTypeRepository : GenericRepository<ListChargeType>, IListChargeTypeRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListChargeTypeRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }

        public IList<ListChargeType> GetAllListChargeType()
        {
            return this.hotelManagerEntities.ListChargeTypes.ToList();
        }
    }
}
