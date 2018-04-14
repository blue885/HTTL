using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCCore.Repositories.PurchaseTasks;
using MVCDTO.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Api.PurchaseTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PurchaseInvoicesApiController : Controller
    {
        private readonly IPurchaseInvoiceRepository purchaseInvoiceRepository;

        public PurchaseInvoicesApiController(IPurchaseInvoiceRepository purchaseInvoiceRepository)
        {
            this.purchaseInvoiceRepository = purchaseInvoiceRepository;
        }

        public JsonResult GetPurchaseInvoices([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<PurchaseInvoice> purchaseInvoices = this.purchaseInvoiceRepository.GetAll();

            DataSourceResult response = purchaseInvoices.ToDataSourceResult(request, o => new PurchaseInvoicePrimitiveDTO
            {
                PurchaseInvoiceID = o.PurchaseInvoiceID,
                PurchaseInvoiceDate = o.PurchaseInvoiceDate,
                PurchaseInvoiceReference = o.PurchaseInvoiceReference,
                TotalQuantity = o.TotalQuantity,
                TotalAmount = o.TotalAmount,
                TotalVATAmount = o.TotalVATAmount,
                TotalGrossAmount = o.TotalGrossAmount,
                SupplierName = o.SupplierName,
                AttentionName = o.AttentionName,
                Description = o.Description,
                Remarks = o.Remarks 
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    

        public JsonResult SearchPurchaseInvoiceBySupplierName(string text)
        {
            var result = purchaseInvoiceRepository.SearchPurchaseInvoiceBySupplierName(text).Select(s => new { s.SupplierName });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
    }
}