using MVCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListRoomTypePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.RoomTypeID; }
        public void SetID(int id) { this.RoomTypeID = id; }

        public int RoomTypeID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string IconIdle { get; set; }
        public string IconBusy { get; set; }    
    }

    public class ListRoomTypeDTO : ListRoomTypePrimitiveDTO
    {
        public ListRoomTypeDTO()
        {

        }
    }       
       
}
