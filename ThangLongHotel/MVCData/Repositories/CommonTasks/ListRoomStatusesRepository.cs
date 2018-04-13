using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{    
    public class ListRoomStatusesRepository : GenericRepository<ListRoomStatuses>, IListRoomStatusesRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListRoomStatusesRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }

        public IList<ListRoomStatuses> GetAllListRoomStatuses()
        {
            return this.hotelManagerEntities.ListRoomStatuses.ToList();
        }

    }
}
