using System;
using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.PurchaseTasks
{
    public interface IPurchaseInvoiceRepository : IGenericWithDetailRepository<PurchaseInvoice, PurchaseInvoiceDetail>
    {
        IList<PurchaseInvoice> SearchPurchaseInvoiceBySupplierName(string name);
        IList<PurchaseInvoice> GetAllSupplierName();
        bool CheckOverStock(DateTime? actionDate, int? purchaseInvoiceID, int? billingID);
        void DeletePurchaseInvoice(int purchaseInvoiceID);
    }
}
