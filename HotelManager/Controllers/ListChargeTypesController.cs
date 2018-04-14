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
using AutoMapper;
using HotelManager.ViewModels.CommonTasks;
using MVCDTO.CommonTasks;
using MVCCore.Services.CommonTasks;
using HotelManager.Builders.CommonTasks;

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ListChargeTypesController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();

        private readonly IListChargeTypeService listChargeTypeService;
        private readonly IListChargeTypeBuilder listChargeTypeBuilder;

        public ListChargeTypesController(IListChargeTypeService listChargeTypeService,
                                        IListChargeTypeBuilder listChargeTypeBuilder)
        {
            this.listChargeTypeService = listChargeTypeService;
            this.listChargeTypeBuilder = listChargeTypeBuilder;
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
        /// This Create action preceded by the [HttpPost] attribute to receive ONLY POST request when user create new ListChargeType
        /// The POST request passes the parameter model: listChargeTypeViewModel to the controller (It init just some properties only. See Index view - the view POST this model - for more information.)
        /// Note: When create new ListChargeType, user have TWO OPTIONS:
        ///     a)Create new ListChargeType with a specific PurchaseOrder
        ///     b)Create new ListChargeType with a specific Supplier
        ///     in two cases: The model binding will map input from view dialog to the parameter model
        /// </summary>
        /// <param name="listChargeTypeGetPurchaseOrders_Result"></param>
        /// <returns></returns>     
        public ActionResult Create()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            ListChargeTypeViewModel listChargeTypeViewModel = new ListChargeTypeViewModel();
            return View(this.TailorViewModel(listChargeTypeViewModel)); //Need to call new PurchaseOrderViewModel() to ensure construct PurchaseOrderViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListChargeTypeViewModel listChargeTypeViewModel)
        {
            if (this.Save(listChargeTypeViewModel))
                return RedirectToAction("Index", new { id = listChargeTypeViewModel.ChargeTypeID });
            else
                return View(this.TailorViewModel(listChargeTypeViewModel));
        }


        public ActionResult Edit(int? id)
        {
            ListChargeType listChargeType;
            if (id == null || (listChargeType = this.listChargeTypeService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ListChargeTypeViewModel listChargeTypeViewModel = Mapper.Map<ListChargeTypeViewModel>(listChargeType);
            return View(this.TailorViewModel(listChargeTypeViewModel));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ListChargeTypeViewModel listChargeTypeViewModel)
        {
            if (this.Save(listChargeTypeViewModel))
                return RedirectToAction("Index", new { id = listChargeTypeViewModel.ChargeTypeID });
            else
            {
                return View(this.TailorViewModel(listChargeTypeViewModel));
            }
        }


        [HttpPost]        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (this.listChargeTypeService.Delete((int)id))
                    return Json(new { Success = 1, ex = "" });
                else
                    return Json(new { Success = 0, ex = "" });
            }
            catch (Exception exception)
            {
                return Json(new { Success = 0, ex = exception.Message });
            }
        }


        private bool Save(ListChargeTypeViewModel listChargeTypeViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                ListChargeTypeDTO listChargeTypeDTO = Mapper.Map<ListChargeTypeViewModel, ListChargeTypeDTO>(listChargeTypeViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(listChargeTypeDTO)) return false;//Check DTO IsValid
                else
                    if (this.listChargeTypeService.Save(listChargeTypeDTO))
                    {
                        listChargeTypeViewModel.ChargeTypeID = listChargeTypeDTO.ChargeTypeID;
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

        private ListChargeTypeViewModel TailorViewModel(ListChargeTypeViewModel listChargeTypeViewModel)
        {
            this.listChargeTypeBuilder.BuildSelectListsForListChargeTypeViewModel(listChargeTypeViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return listChargeTypeViewModel;
        }

        [AllowAnonymous]
        public JsonResult GetListChargeType()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listChargeType = db.ListChargeTypes.OrderBy(o => o.Description).ToList();
            db.Configuration.ProxyCreationEnabled = true;

            return Json(listChargeType, JsonRequestBehavior.AllowGet);
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
