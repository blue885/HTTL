using HotelManager.ViewModels.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders
{
    public interface IPurchaseInvoiceViewModelSelectListBuilder
    {
        void BuildSelectListsForPurchaseInvoiceViewModel(PurchaseInvoiceViewModel purchaseInvoiceViewModel);
        IEnumerable<SelectListItem> BuildSelectListItemsForSuppliers(IEnumerable<PurchaseInvoice> purchaseInvoice);
    }
}