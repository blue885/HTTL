using MVCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Display(Name = "Loại hình")]
        public int HotelID { get; set; }
        [Display(Name = "Loại hình")]
        public string ListHotelDescription { get; set; }
        [Display(Name = "Loại phòng")]
        public int RoomTypeID { get; set; }
        [Display(Name = "Loại phòng")]
        public string ListRoomTypeDescription { get; set; }
        [Display(Name = "Trạng thái phòng")]
        public int RoomStatusID { get; set; }
        [Display(Name = "Trạng thái phòng")]
        public string ListRoomStatusDescription { get; set; }
        [Display(Name = "Tầng")]
        public string FloorLevel { get; set; }
        [Display(Name = "Số giường")]
        public int NoBed { get; set; }
        [Display(Name = "Số người")]
        public int NoPerson { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }
    }

    public class ListRoomDTO : ListRoomPrimitiveDTO
    {
        public ListRoomDTO()
        {
            
        }
    }   
}
