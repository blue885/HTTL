using MVCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListRoomStatusesPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.RoomStatusID; }
        public void SetID(int id) { this.RoomStatusID = id; }

        public int RoomStatusID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string StatusIcon { get; set; }  
    }

    public class ListRoomStatusesDTO : ListRoomStatusesPrimitiveDTO
    {
        public ListRoomStatusesDTO()
        {

        }
    }       
       
}
