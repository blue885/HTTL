using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManager.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HotelManager.ViewModels;
using System.Web.UI;
using Microsoft.AspNet.Identity;

namespace HotelManager.Controllers
{
    
    public partial class HomeController
    {   

        
        public ActionResult Resources_Grouping_Vertical()
        {
            return View();
        }
        
        public virtual JsonResult Grouping_Vertical_Read([DataSourceRequest] DataSourceRequest request)
        {
            var bookingServiceList = schedulerBookingService.GetAll(User.Identity.GetUserId(), Home.GetHotelID(this.HttpContext));
            return Json(bookingServiceList.ToDataSourceResult(request));
        }

        
        public virtual JsonResult Grouping_Vertical_Destroy([DataSourceRequest] DataSourceRequest request, BookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                schedulerBookingService.Delete(booking, ModelState);
            }

            return Json(new[] { booking }.ToDataSourceResult(request, ModelState));
        }

        
        public virtual JsonResult Grouping_Vertical_Create([DataSourceRequest] DataSourceRequest request, BookingViewModel booking)
        {
            if (booking.IsNhanPhong == false)
            {
                if (ModelState.IsValid)
                {
                    booking.Title = booking.CustomerIdentityNo;
                    schedulerBookingService.Insert(booking, ModelState);
                }
            }

            return Json(new[] { booking }.ToDataSourceResult(request, ModelState));
        }

        
        public virtual JsonResult Grouping_Vertical_Update([DataSourceRequest] DataSourceRequest request, BookingViewModel booking)
        {

            if (booking.IsNhanPhong == true)
            {

            }
            else
            {
                if (ModelState.IsValid)
                {
                    booking.Title = booking.CustomerIdentityNo;
                    schedulerBookingService.Update(booking, ModelState);

                }
            }

            return Json(new[] { booking }.ToDataSourceResult(request, ModelState));
        }                      


    }
}