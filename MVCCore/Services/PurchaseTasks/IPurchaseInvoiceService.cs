using MVCModel.Models;
using MVCDTO.PurchaseTasks;
using System.Collections.Generic;

namespace MVCCore.Services.PurchaseTasks
{
    public interface IPurchaseInvoiceService : IGenericWithDetailService<PurchaseInvoice, PurchaseInvoiceDetail, PurchaseInvoiceDTO, PurchaseInvoicePrimitiveDTO, PurchaseInvoiceDetailDTO>
    {
        void DeletePurchaseInvoice(int purchaseInvoiceID);
    }
}
