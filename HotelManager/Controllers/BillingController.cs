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

        public virtual JsonResult GetBillingLog([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            var billingServiceList = schedulerBillingService.GetAll(User.Identity.GetUserId(), Home.GetHotelID(this.HttpContext));
            return Json(billingServiceList.ToDataSourceResult(dataSourceRequest));            
        }            


    }
}