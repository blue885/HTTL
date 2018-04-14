using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using HotelManager.Models;
using System.Web.Mvc;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class SchedulerBillingService : ISchedulerEventService<BillingLogViewModel>
    {
        private HotelManagerEntities db;

        public SchedulerBillingService(HotelManagerEntities context)
        {
            db = context;
        }

        public SchedulerBillingService()
            : this(new HotelManagerEntities())
        {
        }

        public virtual IQueryable<BillingLogViewModel> GetAll()
        {
            return this.GetAll("UnknowUser", 0);
        }


        public virtual IQueryable<BillingLogViewModel> GetAll(string userId, int hotelID)
        {
            List<int> userHotelList = db.AspNetUserHotels.Where(w => w.UserId == userId).Select(s => s.HotelID).ToList();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            var infoQuery = ((from billingMasters in db.BillingMasters
                              where userHotelList.Contains(billingMasters.ListRoom.HotelID)
                              select new
                              {
                                  billingMasters.BillingID,
                                  billingMasters.CustomerName,
                                  billingMasters.CustomerIdentityNo,
                                  billingMasters.TotalAmount,
                                  billingMasters.ArrivalDate,
                                  billingMasters.DepartureDate,
                                  billingMasters.IsCheckOut,
                                  Description = billingMasters.Remarks,
                                  billingMasters.ChargeTypeID,
                                  billingMasters.ListRoom.HotelID,
                                  billingMasters.ListRoom.ListRoomType.RoomCategoryID
                              }).Union

                             (from billingMasters in db.BillingMasters
                              where billingMasters.IsCheckOut && userHotelList.Contains(billingMasters.ListRoom.HotelID)
                              group billingMasters by new { DepartureDate = DbFunctions.TruncateTime(billingMasters.DepartureDate), billingMasters.ListRoom.HotelID } into billingGroupByDate
                              select new
                              {
                                  BillingID = 0,
                                  CustomerName = "",
                                  CustomerIdentityNo = "$",
                                  TotalAmount = billingGroupByDate.Sum(a => a.TotalAmount),
                                  ArrivalDate = billingGroupByDate.Key.DepartureDate.Value,
                                  DepartureDate = billingGroupByDate.Key.DepartureDate,
                                  IsCheckOut = true,
                                  Description = "",
                                  ChargeTypeID = 2,
                                  HotelID = billingGroupByDate.Key.HotelID,
                                  RoomCategoryID = 0
                              })

                             ).ToList().Select(billings => new BillingLogViewModel

            {
                Title = textInfo.ToTitleCase(string.IsNullOrWhiteSpace(billings.CustomerName) ? billings.CustomerIdentityNo.Trim() : GetCustomerLastName(billings.CustomerName.Trim().ToLower())) + " [" + (billings.TotalAmount / 1000).ToString("N0") + "]",
                Start = DateTime.SpecifyKind(billings.BillingID == 0 ? billings.ArrivalDate.AddHours(23).AddMinutes(59).AddSeconds(59) : billings.ArrivalDate, DateTimeKind.Utc),
                End = DateTime.SpecifyKind(billings.IsCheckOut ? (DateTime)(billings.DepartureDate) : DateTime.Now, DateTimeKind.Utc),
                StartTimezone = null,
                EndTimezone = null,
                Description = billings.Description,
                IsAllDay = billings.ChargeTypeID == 2 ? true : false,
                RecurrenceRule = null,
                RecurrenceException = null,
                RecurrenceID = null,                
                BillingID = billings.BillingID,
                HotelID = billings.HotelID,        
                RoomCategoryID = billings.RoomCategoryID,
                OwnerID = billings.BillingID == 0 ? 3 : billings.IsCheckOut == false ? billings.RoomCategoryID + 100 : billings.RoomCategoryID + 200

            }).Where(billing => billing.HotelID == hotelID || hotelID == 0);

            return infoQuery.AsQueryable();
        }


        public virtual void Insert(BillingLogViewModel billing, ModelStateDictionary modelState)
        {

        }

        public virtual void Update(BillingLogViewModel billing, ModelStateDictionary modelState)
        {

        }

        public virtual void Delete(BillingLogViewModel billing, ModelStateDictionary modelState)
        {

        }

        private string GetCustomerLastName(string customerName)
        {
            customerName = customerName.Trim().ToLower();
            string[] words = customerName.Split(' ');
            return words.Count() > 0 ? words[words.Count() - 1] : customerName;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}