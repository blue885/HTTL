using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using HotelManager.Models;
using MVCModel.Models;

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ListHotelsController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();

        // GET: ListHotels
        public ActionResult Index()
        {
            var listHotels = db.ListHotels.Include(l => l.ListLocation);
            return View(listHotels.ToList());
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual JsonResult ListHotel_Read()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            db.Configuration.ProxyCreationEnabled = false;

            var userId = User.Identity.GetUserId();
            List<int> userHotelList = db.AspNetUserHotels.Where(w => w.UserId == userId).Select(s => s.HotelID).ToList();

            var listHotel = db.ListHotels.Where(h => (h.HotelID == hotelID || hotelID == 0) && userHotelList.Contains(h.HotelID)).ToList().AsQueryable();
            db.Configuration.ProxyCreationEnabled = true;
            return Json(listHotel, JsonRequestBehavior.AllowGet);
        }

        // GET: ListHotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListHotel listHotel = db.ListHotels.Find(id);
            if (listHotel == null)
            {
                return HttpNotFound();
            }
            return View(listHotel);
        }

        // GET: ListHotels/Create
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.ListLocations, "LocationID", "Description");
            return View();
        }

        // POST: ListHotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelID,Description,LocationID,Remarks,HotelOrderNo,Name")] ListHotel listHotel)
        {
            if (ModelState.IsValid)
            {
                db.ListHotels.Add(listHotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.ListLocations, "LocationID", "Description", listHotel.LocationID);
            return View(listHotel);
        }

        // GET: ListHotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListHotel listHotel = db.ListHotels.Find(id);
            if (listHotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.ListLocations, "LocationID", "Description", listHotel.LocationID);
            return View(listHotel);
        }

        // POST: ListHotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelID,Description,LocationID,Remarks,HotelOrderNo,Name")] ListHotel listHotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listHotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.ListLocations, "LocationID", "Description", listHotel.LocationID);
            return View(listHotel);
        }

        // GET: ListHotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListHotel listHotel = db.ListHotels.Find(id);
            if (listHotel == null)
            {
                return HttpNotFound();
            }
            return View(listHotel);
        }

        // POST: ListHotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListHotel listHotel = db.ListHotels.Find(id);
            db.ListHotels.Remove(listHotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
