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
using AutoMapper;
using MVCDTO.CommonTasks;
using MVCCore.Services.CommonTasks;
using HotelManager.Builders.CommonTasks;

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ListServicesController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();

        private readonly IListServiceService listServiceService;
        private readonly IListServiceBuilder listServiceBuilder;

        public ListServicesController(IListServiceService listServiceService,
                                        IListServiceBuilder listServiceBuilder)
        {
            this.listServiceService = listServiceService;
            this.listServiceBuilder = listServiceBuilder;
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
        /// This Create action preceded by the [HttpPost] attribute to receive ONLY POST request when user create new ListService
        /// The POST request passes the parameter model: listServiceViewModel to the controller (It init just some properties only. See Index view - the view POST this model - for more information.)
        /// Note: When create new ListService, user have TWO OPTIONS:
        ///     a)Create new ListService with a specific PurchaseOrder
        ///     b)Create new ListService with a specific Supplier
        ///     in two cases: The model binding will map input from view dialog to the parameter model
        /// </summary>
        /// <param name="listServiceGetPurchaseOrders_Result"></param>
        /// <returns></returns>     
        public ActionResult Create()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            ListServiceViewModel listServiceViewModel = new ListServiceViewModel();
            return View(this.TailorViewModel(listServiceViewModel)); //Need to call new PurchaseOrderViewModel() to ensure construct PurchaseOrderViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListServiceViewModel listServiceViewModel)
        {
            if (this.Save(listServiceViewModel))
                return RedirectToAction("Index", new { id = listServiceViewModel.ServiceID });
            else
                return View(this.TailorViewModel(listServiceViewModel));
        }


        public ActionResult Edit(int? id)
        {
            ListService listService;
            if (id == null || (listService = this.listServiceService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ListServiceViewModel listServiceViewModel = Mapper.Map<ListServiceViewModel>(listService);
            return View(this.TailorViewModel(listServiceViewModel));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ListServiceViewModel listServiceViewModel)
        {
            if (this.Save(listServiceViewModel))
                return RedirectToAction("Index", new { id = listServiceViewModel.ServiceID });
            else
            {
                return View(this.TailorViewModel(listServiceViewModel));
            }
        }


        [HttpPost]        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (this.listServiceService.Delete((int)id))
                    return Json(new { Success = 1, ex = "" });
                else
                    return Json(new { Success = 0, ex = "" });
            }
            catch (Exception exception)
            {
                return Json(new { Success = 0, ex = exception.Message });
            }
        }


        private bool Save(ListServiceViewModel listServiceViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                ListServiceDTO listServiceDTO = Mapper.Map<ListServiceViewModel, ListServiceDTO>(listServiceViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(listServiceDTO)) return false;//Check DTO IsValid
                else
                    if (this.listServiceService.Save(listServiceDTO))
                    {
                        listServiceViewModel.ServiceID = listServiceDTO.ServiceID;
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

        private ListServiceViewModel TailorViewModel(ListServiceViewModel listServiceViewModel)
        {
            this.listServiceBuilder.BuildSelectListsForListServiceViewModel(listServiceViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return listServiceViewModel;
        }

        [AllowAnonymous]
        public JsonResult GetListService()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listService = db.ListServices.OrderBy(z => z.Description).ToList();
            db.Configuration.ProxyCreationEnabled = true;
            return Json(listService.ToList(), JsonRequestBehavior.AllowGet);
        }
       
    }
}
