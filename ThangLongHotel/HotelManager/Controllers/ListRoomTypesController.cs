using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManager.Models;

using MVCModel.Models;

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ListRoomTypesController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();

        // GET: ListRoomTypes
        public ActionResult Index()
        {
            return View(db.ListRoomTypes.ToList());
        }

        [AllowAnonymous]
        public virtual JsonResult ListRoomType_Read()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listRoomTypes = db.ListRoomTypes.ToList().AsQueryable();
            db.Configuration.ProxyCreationEnabled = true;
            return Json(listRoomTypes, JsonRequestBehavior.AllowGet);
        }

        // GET: ListRoomTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListRoomType listRoomType = db.ListRoomTypes.Find(id);
            if (listRoomType == null)
            {
                return HttpNotFound();
            }
            return View(listRoomType);
        }

        // GET: ListRoomTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListRoomTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomTypeID,Description,Remarks,IconIdle,IconBusy")] ListRoomType listRoomType)
        {
            if (ModelState.IsValid)
            {
                db.ListRoomTypes.Add(listRoomType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listRoomType);
        }

        // GET: ListRoomTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListRoomType listRoomType = db.ListRoomTypes.Find(id);
            if (listRoomType == null)
            {
                return HttpNotFound();
            }
            return View(listRoomType);
        }

        // POST: ListRoomTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomTypeID,Description,Remarks,IconIdle,IconBusy")] ListRoomType listRoomType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listRoomType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(listRoomType);
        }

        // GET: ListRoomTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListRoomType listRoomType = db.ListRoomTypes.Find(id);
            if (listRoomType == null)
            {
                return HttpNotFound();
            }
            return View(listRoomType);
        }

        // POST: ListRoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListRoomType listRoomType = db.ListRoomTypes.Find(id);
            db.ListRoomTypes.Remove(listRoomType);
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
