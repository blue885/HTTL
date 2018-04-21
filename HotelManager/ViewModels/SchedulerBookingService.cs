using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using HotelManager.Models;
using System.Web.Mvc;

using MVCModel.Models;

namespace HotelManager.ViewModels
{
    public class SchedulerBookingService : ISchedulerEventService<BookingViewModel>
    {
        private HotelManagerEntities db;

        public SchedulerBookingService(HotelManagerEntities context)
        {
            db = context;
        }

        public SchedulerBookingService()
            : this(new HotelManagerEntities())
        {
        }

        public virtual IQueryable<BookingViewModel> GetAll()
        {
            return this.GetAll("UnknowUser", 0);
        }

        public virtual IQueryable<BookingViewModel> GetAll(string userId, int hotelID)
        {
            List<int> userHotelList = db.AspNetUserHotels.Where(w => w.UserId == userId).Select(s => s.HotelID).ToList();
            var bookingMaster = db.BookingMasters.ToList().Select(booking => new BookingViewModel
            {
                BookingID = booking.BookingID,
                Title = booking.Title,                
                //Start = DateTime.SpecifyKind(booking.ArrivalDate, DateTimeKind.Utc),
                //End = DateTime.SpecifyKind(booking.DepartureDate, DateTimeKind.Utc),
                Start = booking.ArrivalDate,
                End = booking.DepartureDate,
                StartTimezone = booking.StartTimezone,
                EndTimezone = booking.EndTimezone,
                Description = booking.Description,
                IsAllDay = booking.IsAllDay,
                RecurrenceRule = booking.RecurrenceRule,
                RecurrenceException = booking.RecurrenceException,
                RecurrenceID = booking.RecurrenceID,
                BookingDate = booking.BookingDate,
                BookingReference = booking.BookingReference,
                HotelID = booking.HotelID,
                RoomTypeID = booking.RoomTypeID,
                RoomID = 0,
                IsNhanPhong = false,
                CustomerName = booking.CustomerName,
                CustomerIdentityNo = booking.CustomerIdentityNo,
                CustomerSex = booking.CustomerSex,
                CustomerAddress = booking.CustomerAddress,
                CustomerTelephone = booking.CustomerTelephone,
                CustomerNationality = booking.CustomerNationality,
                ChargeTypeID = booking.ChargeTypeID,
                AdvancePayment = booking.AdvancePayment,
                IsPaidByATMCard = booking.IsPaidByATMCard,
                Remarks = booking.Remarks,
                InActive = booking.InActive
            }).Where(booking => booking.InActive == false && (booking.HotelID == hotelID || hotelID == 0) && userHotelList.Contains(booking.HotelID));

            return bookingMaster.AsQueryable();

        }


        public virtual void Insert(BookingViewModel booking, ModelStateDictionary modelState)
        {
            if (ValidateModel(booking, modelState))
            {

                if (string.IsNullOrEmpty(booking.Title))
                {
                    booking.Title = "";
                }

                var entity = booking.ToEntity();

                db.BookingMasters.Add(entity);
                db.SaveChanges();

                booking.BookingID = entity.BookingID;
            }
        }

        public virtual void Update(BookingViewModel booking, ModelStateDictionary modelState)
        {
            if (ValidateModel(booking, modelState))
            {
                if (string.IsNullOrEmpty(booking.Title))
                {
                    booking.Title = "";
                }

                var entity = db.BookingMasters.FirstOrDefault(m => m.BookingID == booking.BookingID);

                entity.Title = booking.Title;
                entity.ArrivalDate = booking.Start;
                entity.DepartureDate = booking.End;
                entity.Description = booking.Description;
                entity.IsAllDay = booking.IsAllDay;
                entity.RecurrenceID = booking.RecurrenceID;
                entity.RecurrenceRule = booking.RecurrenceRule;
                entity.RecurrenceException = booking.RecurrenceException;
                entity.StartTimezone = booking.StartTimezone;
                entity.EndTimezone = booking.EndTimezone;
                entity.BookingDate = DateTime.Now;
                entity.BookingReference = "#";
                entity.HotelID = booking.HotelID;
                entity.RoomTypeID = booking.RoomTypeID;
                entity.CustomerName = booking.CustomerName;
                entity.CustomerIdentityNo = booking.CustomerIdentityNo;
                entity.CustomerSex = booking.CustomerSex;
                entity.CustomerAddress = booking.CustomerAddress;
                entity.CustomerTelephone = booking.CustomerTelephone;
                entity.CustomerNationality = booking.CustomerNationality;
                entity.ChargeTypeID = (int)booking.ChargeTypeID;
                entity.AdvancePayment = booking.AdvancePayment;
                entity.IsPaidByATMCard = booking.IsPaidByATMCard;
                entity.Remarks = booking.Remarks ?? "";
                entity.InActive = booking.InActive;

                db.SaveChanges();
            }
        }

        public virtual void Delete(BookingViewModel booking, ModelStateDictionary modelState)
        {
            var entity = db.BookingMasters.FirstOrDefault(m => m.BookingID == booking.BookingID);

            entity.InActive = true;

            db.SaveChanges();
        }

        private bool ValidateModel(BookingViewModel appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }    
}