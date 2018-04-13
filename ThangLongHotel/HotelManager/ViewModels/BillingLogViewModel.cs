using HotelManager.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class BillingLogViewModel : ISchedulerEvent
    {
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string Description { get; set; }
        public string RecurrenceRule { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        
        public int BillingID { get; set; }
        public int HotelID { get; set; }
        public int RoomCategoryID { get; set; }    
        public int OwnerID { get; set; }
    }
}