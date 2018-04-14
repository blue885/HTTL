using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{
    public class ListServiceRepository : GenericRepository<ListService>, IListServiceRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListServiceRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }

        public IList<ListService> SearchServicesByName(string name)
        {
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = false;
            List<ListService> commodities = this.hotelManagerEntities.ListServices.Where(w => w.Description.Contains(name)).ToList();
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = true;

            return commodities;
        }
    }
}
