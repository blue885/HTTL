using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{    
    public class ListRoomRepository : GenericRepository<ListRoom>, IListRoomRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListRoomRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }


    }
}
