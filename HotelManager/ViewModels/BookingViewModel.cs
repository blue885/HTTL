using HotelManager.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class BookingViewModel : ISchedulerEvent
    {
        public int BookingID { get; set; }
        public System.DateTime BookingDate { get; set; }
        public string BookingReference { get; set; }
        public int HotelID { get; set; }
        public int RoomTypeID { get; set; }
        
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
        public bool IsAllDay { get; set; }
        private System.DateTime start { get; set; }
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }
        private System.DateTime end { get; set; }
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdentityNo { get; set; }
        public Nullable<bool> CustomerSex { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerNationality { get; set; }
        public int? ChargeTypeID { get; set; }
        public int RoomID { get; set; }
        public double AdvancePayment { get; set; }
        public bool IsPaidByATMCard { get; set; }
        public bool IsNhanPhong { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string RecurrenceRule { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool InActive { get; set; }
        public List<ListHotel> ListHotels { get; set; }
        public List<ListRoomType> ListRoomTypes { get; set; }

        public BookingMaster ToEntity()
        {
            var bookingMaster = new BookingMaster
            {
                BookingID = BookingID,
                Title = Title,
                IsAllDay = IsAllDay,
                ArrivalDate = Start,
                DepartureDate = End,
                StartTimezone = StartTimezone,
                EndTimezone = EndTimezone,
                CustomerName = CustomerName,
                CustomerIdentityNo = CustomerIdentityNo,
                CustomerSex = CustomerSex,
                CustomerAddress = CustomerAddress,
                CustomerTelephone = CustomerTelephone,
                CustomerNationality = CustomerNationality,
                HotelID = HotelID,
                RoomTypeID = RoomTypeID,
                ChargeTypeID = (int)ChargeTypeID,
                BookingDate = DateTime.Now,
                BookingReference = "#",
                IsPaidByATMCard = false,
                AdvancePayment = AdvancePayment,
                Description = Description,
                Remarks = Remarks ?? "",
                RecurrenceRule = RecurrenceRule,
                RecurrenceID = RecurrenceID,
                RecurrenceException = RecurrenceException,
                InActive = InActive
            };
            return bookingMaster;
        }
    }
}