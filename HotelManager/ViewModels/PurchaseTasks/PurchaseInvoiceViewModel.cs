using MVCDTO.PurchaseTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.ViewModels.PurchaseTasks
{
    public class PurchaseInvoiceViewModel : PurchaseInvoiceDTO
    {
        public IEnumerable<SelectListItem> HotelSelectList { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectList { get; set; }
    }
}