using HotelManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<HotelFloorLevel> HotelFloorLevels { get; set; }
        public List<HotelRoom> HotelRooms { get; set; }
        //public IEnumerable<BillingList> BillingLists { get; set; }
        public IEnumerable<ListContextMenu> ListContextMenus { get; set; }
    }
}