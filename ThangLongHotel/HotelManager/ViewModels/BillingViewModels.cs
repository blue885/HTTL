using HotelManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class BillingViewModels
    {
        public int HotelID { get; set; }
        public int RoomCategoryID { get; set; }

        public BillingMaster BillingMaster { get; set; }
        public IEnumerable<BillingDetailFull> BillingDetailFulls { get; set; }
    }
}