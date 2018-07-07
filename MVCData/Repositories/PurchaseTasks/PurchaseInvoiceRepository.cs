using System;
using System.Collections.Generic;
using System.Linq;

using MVCModel.Models;
using MVCCore.Repositories.PurchaseTasks;
using MVCData.Repositories.CommonTasks;


using MVCData.Helpers;

namespace MVCData.Repositories.PurchaseTasks
{
    public class PurchaseInvoiceRepository : GenericWithDetailRepository<PurchaseInvoice, PurchaseInvoiceDetail>, IPurchaseInvoiceRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public PurchaseInvoiceRepository(HotelManagerEntities hotelManagerEntities)
            : base(hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;

            //SqlRoutines x = new SqlRoutines(this.hotelManagerEntities);

            //x.RestoreProcedure();

        }

        public bool CheckOverStock(DateTime? actionDate, int? purchaseInvoiceID, int? billingID)
        {
            WarehouseRepository warehouseRepository = new WarehouseRepository(this.hotelManagerEntities);
            return warehouseRepository.CheckOverStock(actionDate, purchaseInvoiceID, billingID);
        }

        public IList<PurchaseInvoice> SearchPurchaseInvoiceBySupplierName(string name)
        {
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = false;
            List<PurchaseInvoice> purchaseInvoices = this.hotelManagerEntities.PurchaseInvoices.Where(w => w.SupplierName.Contains(name)).GroupBy(group => group.SupplierName).Select(s => s.FirstOrDefault()).ToList();
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = true;

            return purchaseInvoices;
        }

        public IList<PurchaseInvoice> GetAllSupplierName()
        {
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = false;
            List<PurchaseInvoice> purchaseInvoices = this.hotelManagerEntities.PurchaseInvoices.GroupBy(group => group.SupplierName).Select(s => s.FirstOrDefault()).ToList();
            this.hotelManagerEntities.Configuration.ProxyCreationEnabled = true;

            return purchaseInvoices;
        }

        public void DeletePurchaseInvoice(int purchaseInvoiceID)
        {
            using (var dbContextTransaction = hotelManagerEntities.Database.BeginTransaction())
            {
                try
                {
                    var purchaseIvoiceDetail = this.hotelManagerEntities.PurchaseInvoiceDetails.Where(w => w.PurchaseInvoiceID == purchaseInvoiceID);
                    this.hotelManagerEntities.PurchaseInvoiceDetails.RemoveRange(purchaseIvoiceDetail);
                    var purchaseIvoice = this.hotelManagerEntities.PurchaseInvoices.Where(w => w.PurchaseInvoiceID == purchaseInvoiceID).FirstOrDefault();
                    this.hotelManagerEntities.PurchaseInvoices.Remove(purchaseIvoice);
                    this.hotelManagerEntities.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }

        }

    }
}
