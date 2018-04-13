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
using HotelManager.ViewModels.CommonTasks;
using MVCDTO.CommonTasks;
using AutoMapper;
using MVCCore.Services.CommonTasks;
using HotelManager.Builders.CommonTasks;
using HotelManager.Configuration;

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ListRoomsController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();
        private readonly IListRoomService listRoomService;
        private readonly IListRoomBuilder listRoomBuilder;

        public ListRoomsController(IListRoomService listRoomService,
                                        IListRoomBuilder listRoomBuilder)
        {
            this.listRoomService = listRoomService;
            this.listRoomBuilder = listRoomBuilder;
        }


        public ActionResult Index(int? id)
        {
            ViewBag.SelectedID = id == null ? -1 : (int)id;
            return View();
        }

        public ActionResult PopupOption()
        {
            return View();
        }


        /// <summary>
        /// This Create action preceded by the [HttpPost] attribute to receive ONLY POST request when user create new ListRoom
        /// The POST request passes the parameter model: listRoomViewModel to the controller (It init just some properties only. See Index view - the view POST this model - for more information.)
        /// Note: When create new ListRoom, user have TWO OPTIONS:
        ///     a)Create new ListRoom with a specific PurchaseOrder
        ///     b)Create new ListRoom with a specific Supplier
        ///     in two cases: The model binding will map input from view dialog to the parameter model
        /// </summary>
        /// <param name="listRoomGetPurchaseOrders_Result"></param>
        /// <returns></returns>     
        public ActionResult Create()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            ListRoomViewModel listRoomViewModel = new ListRoomViewModel();
            return View(this.TailorViewModel(listRoomViewModel)); //Need to call new PurchaseOrderViewModel() to ensure construct PurchaseOrderViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListRoomViewModel listRoomViewModel)
        {
            if (this.Save(listRoomViewModel))
                return RedirectToAction("Index", new { id = listRoomViewModel.RoomID });
            else
                return View(this.TailorViewModel(listRoomViewModel));
        }


        public ActionResult Edit(int? id)
        {
            ListRoom listRoom;
            if (id == null || (listRoom = this.listRoomService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ListRoomViewModel listRoomViewModel = Mapper.Map<ListRoomViewModel>(listRoom);
            return View(this.TailorViewModel(listRoomViewModel));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ListRoomViewModel listRoomViewModel)
        {
            if (this.Save(listRoomViewModel))
                return RedirectToAction("Index", new { id = listRoomViewModel.RoomID });
            else
            {
                return View(this.TailorViewModel(listRoomViewModel));
            }
        }


        [HttpPost]        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (this.listRoomService.Delete((int)id))
                    return Json(new { Success = 1, ex = "" });
                else
                    return Json(new { Success = 0, ex = "" });
            }
            catch (Exception exception)
            {
                return Json(new { Success = 0, ex = exception.Message });
            }
        }


        private bool Save(ListRoomViewModel listRoomViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                ListRoomDTO listRoomDTO = Mapper.Map<ListRoomViewModel, ListRoomDTO>(listRoomViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(listRoomDTO)) return false;//Check DTO IsValid
                else
                    if (this.listRoomService.Save(listRoomDTO))
                    {
                        listRoomViewModel.RoomID = listRoomDTO.RoomID;
                        return true;
                    }
                    else
                        return false;
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return false;
            }
        }

        private ListRoomViewModel TailorViewModel(ListRoomViewModel listRoomViewModel)
        {
            this.listRoomBuilder.BuildSelectListsForListRoomViewModel(listRoomViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return listRoomViewModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public int SetRoomStatus(int roomID, int roomStatusID)
        {
            db.SetRoomStatus(roomID, roomStatusID);
            return 1;
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetActiveRoom(int hotelID, int roomCategoryID, int roomID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var activeRooms = db.GetActiveRoom(hotelID, roomCategoryID, roomID).ToList();
            db.Configuration.ProxyCreationEnabled = true;

            return Json(activeRooms, JsonRequestBehavior.AllowGet);
        }



        [AllowAnonymous]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public int GetRoomCategoryID(int roomTypeID)
        {
            var roomCategoryID = db.ListRoomTypes.FirstOrDefault(w => w.RoomTypeID == roomTypeID).RoomCategoryID;
            return (int)roomCategoryID;
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
