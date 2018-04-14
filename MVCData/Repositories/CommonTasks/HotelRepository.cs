using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{   
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public HotelRepository(HotelManagerEntities hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
        }

        public IList<ListHotel> GetAllHotel()
        {
            return this.hotelManagerEntities.ListHotels.ToList();
        }
    }
}
