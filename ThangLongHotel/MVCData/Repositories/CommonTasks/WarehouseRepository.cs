using System;
using System.Collections.Generic;
using System.Linq;

using MVCModel.Models;
using MVCModel.Validations;

namespace MVCData.Repositories.CommonTasks
{
    public class WarehouseRepository
    {
        private readonly HotelManagerEntities hotelManagerEntities;

        public WarehouseRepository(HotelManagerEntities hotelManagerEntities)
        {
            this.hotelManagerEntities = hotelManagerEntities;
        }

        public bool CheckOverStock(DateTime? actionDate, int? purchaseInvoiceID, int? billingID)
        {
            List<OverStockItem> overStockItems = this.hotelManagerEntities.GetOverStockItems(actionDate, purchaseInvoiceID, billingID).ToList();
            if (overStockItems.Count == 0)
                return true;
            else
            {
                string errorMessage = "Tính đến ngày: " + overStockItems[0].OverStockDate.ToShortDateString() + ", những mặt hàng sau không còn đủ số lượng tồn kho:" + "\r\n" + "\r\n";
                foreach (OverStockItem overStockItem in overStockItems)
                    errorMessage = errorMessage + "\t" + overStockItem.Description + "\t" + "[" + overStockItem.Quantity + "]\t" + "\r\n";

                throw new Exception(errorMessage);
            }

        }
    }
}
