using HotelManager.ViewModels.PurchaseTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Repositories.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders
{
    public class PurchaseInvoiceViewModelSelectListBuilder : IPurchaseInvoiceViewModelSelectListBuilder
    {
        private readonly IHotelSelectListBuilder hotelSelectListBuilder;
        private readonly IHotelRepository hotelRepository;
        private readonly IPurchaseInvoiceRepository purchaseInvoiceRepository;

        public PurchaseInvoiceViewModelSelectListBuilder(IHotelSelectListBuilder hotelSelectListBuilder,
                                            IHotelRepository hotelRepository,
                                            IPurchaseInvoiceRepository purchaseInvoiceRepository)
        {            
            this.hotelSelectListBuilder = hotelSelectListBuilder;
            this.hotelRepository = hotelRepository;
            this.purchaseInvoiceRepository = purchaseInvoiceRepository;
        }

        public void BuildSelectListsForPurchaseInvoiceViewModel(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            purchaseInvoiceViewModel.HotelSelectList = hotelSelectListBuilder.BuildSelectListItemsForHotels(hotelRepository.GetAllHotel());
            purchaseInvoiceViewModel.SupplierSelectList = BuildSelectListItemsForSuppliers(purchaseInvoiceRepository.GetAllSupplierName());
        }

        public IEnumerable<SelectListItem> BuildSelectListItemsForSuppliers(IEnumerable<PurchaseInvoice> purchaseInvoice)
        {
            return purchaseInvoice.Select(pt => new SelectListItem { Text = pt.SupplierName, Value = pt.SupplierName.ToString() }).ToList();
        }
    }
}