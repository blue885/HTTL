using MVCDTO.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.ViewModels.CommonTasks
{

    public class ListRoomViewModel : ListRoomDTO
    {
        public IEnumerable<SelectListItem> HotelSelectList { get; set; }
        public IEnumerable<SelectListItem> RoomTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> RoomStatusesSelectList { get; set; }
    }
}