using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{    
    public class ListRoomTypeRepository : GenericRepository<ListRoomType>, IListRoomTypeRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public ListRoomTypeRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
         

        }

        public IList<ListRoomType> GetAllListRoomType()
        {
            return this.hotelManagerEntities.ListRoomTypes.ToList();
        }

    }
}
