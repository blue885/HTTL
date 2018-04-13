using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MVCModel;

namespace MVCDTO.PurchaseTasks
{
    public class PurchaseInvoiceDetailDTO : BaseModel, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseInvoiceDetailID; }

        public int PurchaseInvoiceDetailID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        [Required(ErrorMessage = "Please select Serviceid")]
        public int ServiceID { get; set; }       

        [UIHint("NMVN/ServiceAutoComplete")]
        [Required(ErrorMessage = "Vui lòng chọn hàng hóa")]
        public string ListServiceDescription { get; set; }

        [UIHint("Decimal")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public decimal Quantity { get; set; }
        public decimal QuantityInvoice { get; set; }
        [UIHint("Decimal")]
        [Range(0, 99999999999, ErrorMessage = "Đơn giá không hợp lệ")]
        public decimal UnitPrice { get; set; }
        [UIHint("DecimalReadonly")]
        public decimal Amount { get; set; }
        [UIHint("Decimal")]
        public decimal VATPercent { get; set; }
        [UIHint("DecimalReadonly")]
        public decimal VATAmount { get; set; }
        [UIHint("DecimalReadonly")]
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }

    }
}
