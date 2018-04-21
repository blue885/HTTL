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
        public int TongPhongTrongOK { get; set; }
        public int TongPhongDangCoKhach { get; set; }
        public int TongPhongDonPhong { get; set; }
        public int TongPhongSuaChua { get; set; }
        public IEnumerable<HotelFloorLevel> HotelFloorLevels { get; set; }
        public List<HotelRoom> HotelRooms { get; set; }
        //public IEnumerable<BillingList> BillingLists { get; set; }
        public IEnumerable<ListContextMenu> ListContextMenus { get; set; }
    }
}