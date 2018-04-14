using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{    
    public class ListChargeRateRepository : GenericRepository<ListChargeRate>, IListChargeRateRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListChargeRateRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }


    }
}
