using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BillingReport()
        {
            return View();
        }

        public ActionResult WarehouseJournal()
        {
            return View();
        }
    }
}