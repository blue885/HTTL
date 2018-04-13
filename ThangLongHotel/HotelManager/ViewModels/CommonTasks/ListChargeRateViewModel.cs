using MVCDTO.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.ViewModels.CommonTasks
{

    public class ListChargeRateViewModel : ListChargeRateDTO
    {
        public IEnumerable<SelectListItem> RoomTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeSelectList { get; set; }
    }
}