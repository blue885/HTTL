using MVCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListRoomPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.RoomID; }
        public void SetID(int id) { this.RoomID = id; }

        public int RoomID { get; set; }

        public string Description { get; set; }
        public int HotelID { get; set; }
        public string ListHotelDescription { get; set; }
        public int RoomTypeID { get; set; }
        public string ListRoomTypeDescription { get; set; }
        public int RoomStatusID { get; set; }
        public string ListRoomStatusDescription { get; set; }
        public string FloorLevel { get; set; }
        public int NoBed { get; set; }
        public int NoPerson { get; set; }
        public string Remarks { get; set; }
    }

    public class ListRoomDTO : ListRoomPrimitiveDTO
    {
        public ListRoomDTO()
        {
            
        }
    }   
}
