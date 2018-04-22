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

namespace HotelManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ListChargeRatesController : Controller
    {
         private readonly IListChargeRateService listChargeRateService;
         private readonly IListChargeRateBuilder listChargeRateBuilder;

        public ListChargeRatesController(IListChargeRateService listChargeRateService,
                                        IListChargeRateBuilder listChargeRateBuilder)
        {
            this.listChargeRateService = listChargeRateService;
            this.listChargeRateBuilder = listChargeRateBuilder;
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
        /// This Create action preceded by the [HttpPost] attribute to receive ONLY POST request when user create new ListChargeRate
        /// The POST request passes the parameter model: listChargeRateViewModel to the controller (It init just some properties only. See Index view - the view POST this model - for more information.)
        /// Note: When create new ListChargeRate, user have TWO OPTIONS:
        ///     a)Create new ListChargeRate with a specific PurchaseOrder
        ///     b)Create new ListChargeRate with a specific Supplier
        ///     in two cases: The model binding will map input from view dialog to the parameter model
        /// </summary>
        /// <param name="listChargeRateGetPurchaseOrders_Result"></param>
        /// <returns></returns>     
        public ActionResult Create()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            ListChargeRateViewModel listChargeRateViewModel = new ListChargeRateViewModel();
            listChargeRateViewModel.ChargeVolumn = 1;
            return View(this.TailorViewModel(listChargeRateViewModel)); //Need to call new PurchaseOrderViewModel() to ensure construct PurchaseOrderViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListChargeRateViewModel listChargeRateViewModel)
        {
            if (this.Save(listChargeRateViewModel))
                return RedirectToAction("Index", new { id = listChargeRateViewModel.ChargeRateID });
            else
                return View(this.TailorViewModel(listChargeRateViewModel));
        }


        public ActionResult Edit(int? id)
        {
            ListChargeRate listChargeRate;
            if (id == null || (listChargeRate = this.listChargeRateService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ListChargeRateViewModel listChargeRateViewModel = Mapper.Map<ListChargeRateViewModel>(listChargeRate);
            return View(this.TailorViewModel(listChargeRateViewModel));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ListChargeRateViewModel listChargeRateViewModel)
        {
            if (this.Save(listChargeRateViewModel))
                return RedirectToAction("Index", new { id = listChargeRateViewModel.ChargeRateID });
            else
            {
                return View(this.TailorViewModel(listChargeRateViewModel));
            }
        }


        [HttpPost]        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {            
            try
            {
                if (this.listChargeRateService.Delete((int)id))
                    return Json(new { Success = 1, ex = "" });
                else
                    return Json(new { Success = 0, ex = "" });    
            }
            catch (Exception exception)
            {
                return Json(new { Success = 0, ex = exception.Message });
            }
        }


        private bool Save(ListChargeRateViewModel listChargeRateViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                ListChargeRateDTO listChargeRateDTO = Mapper.Map<ListChargeRateViewModel, ListChargeRateDTO>(listChargeRateViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(listChargeRateDTO)) return false;//Check DTO IsValid
                else
                    if (this.listChargeRateService.Save(listChargeRateDTO))
                    {
                        listChargeRateViewModel.ChargeRateID = listChargeRateDTO.ChargeRateID;
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

        private ListChargeRateViewModel TailorViewModel(ListChargeRateViewModel listChargeRateViewModel)
        {
            this.listChargeRateBuilder.BuildSelectListsForListChargeRateViewModel(listChargeRateViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return listChargeRateViewModel;
        }        
    }
}
