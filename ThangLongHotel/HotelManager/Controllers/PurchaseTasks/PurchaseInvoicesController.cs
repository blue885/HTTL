using AutoMapper;
using HotelManager.Builders;
using HotelManager.Models;
using HotelManager.ViewModels;
using HotelManager.ViewModels.PurchaseTasks;
using MVCCore.Services.PurchaseTasks;
using MVCDTO.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Controllers.PurchaseTasks
{
    public class PurchaseInvoicesController : Controller
    {
        private readonly IPurchaseInvoiceService purchaseInvoiceService;
        private readonly IPurchaseInvoiceViewModelSelectListBuilder purchaseInvoiceViewModelSelectListBuilder;

        public PurchaseInvoicesController(IPurchaseInvoiceService purchaseInvoiceService,
                                        IPurchaseInvoiceViewModelSelectListBuilder purchaseInvoiceViewModelSelectListBuilder)
        {
            this.purchaseInvoiceService = purchaseInvoiceService;
            this.purchaseInvoiceViewModelSelectListBuilder = purchaseInvoiceViewModelSelectListBuilder;
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
        /// This Create action preceded by the [HttpPost] attribute to receive ONLY POST request when user create new PurchaseInvoice
        /// The POST request passes the parameter model: purchaseInvoiceViewModel to the controller (It init just some properties only. See Index view - the view POST this model - for more information.)
        /// Note: When create new PurchaseInvoice, user have TWO OPTIONS:
        ///     a)Create new PurchaseInvoice with a specific PurchaseOrder
        ///     b)Create new PurchaseInvoice with a specific Supplier
        ///     in two cases: The model binding will map input from view dialog to the parameter model
        /// </summary>
        /// <param name="purchaseInvoiceGetPurchaseOrders_Result"></param>
        /// <returns></returns>     
        public ActionResult Create()
        {
            int hotelID = Home.GetHotelID(this.HttpContext);
            PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
            purchaseInvoiceViewModel.HotelID = hotelID;
            purchaseInvoiceViewModel.PurchaseInvoiceDate = DateTime.Now;

            return View(this.TailorViewModel(purchaseInvoiceViewModel)); //Need to call new PurchaseOrderViewModel() to ensure construct PurchaseOrderViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            if (this.Save(purchaseInvoiceViewModel))
                return RedirectToAction("Index", new { id = purchaseInvoiceViewModel.PurchaseInvoiceID });
            else
                return View(this.TailorViewModel(purchaseInvoiceViewModel));
        }


        public ActionResult Edit(int? id)
        {
            PurchaseInvoice purchaseInvoice;
            if (id == null || (purchaseInvoice = this.purchaseInvoiceService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PurchaseInvoiceViewModel purchaseInvoiceViewModel = Mapper.Map<PurchaseInvoiceViewModel>(purchaseInvoice);
            return View(this.TailorViewModel(purchaseInvoiceViewModel));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            if (this.Save(purchaseInvoiceViewModel))
                return RedirectToAction("Index", new { id = purchaseInvoiceViewModel.PurchaseInvoiceID });
            else
            {
                return View(this.TailorViewModel(purchaseInvoiceViewModel));
            }
        }


        [HttpPost]        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {            
            try
            {
                if (this.purchaseInvoiceService.Delete((int)id))
                    return Json(new { Success = 1, ex = "" });
                else
                    return Json(new { Success = 0, ex = "" });
                //this.purchaseInvoiceService.DeletePurchaseInvoice((int)id);
                //return Json(new { Success = 1, ex = "" });
            }
            catch (Exception exception)
            {
                return Json(new { Success = 0, ex = exception.Message });
            }
        }


        private bool Save(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                PurchaseInvoiceDTO purchaseInvoiceDTO = Mapper.Map<PurchaseInvoiceViewModel, PurchaseInvoiceDTO>(purchaseInvoiceViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(purchaseInvoiceDTO)) return false;//Check DTO IsValid
                else
                    if (this.purchaseInvoiceService.Save(purchaseInvoiceDTO))
                    {
                        purchaseInvoiceViewModel.PurchaseInvoiceID = purchaseInvoiceDTO.PurchaseInvoiceID;
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

        private PurchaseInvoiceViewModel TailorViewModel(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            this.purchaseInvoiceViewModelSelectListBuilder.BuildSelectListsForPurchaseInvoiceViewModel(purchaseInvoiceViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return purchaseInvoiceViewModel;
        }
    }
}