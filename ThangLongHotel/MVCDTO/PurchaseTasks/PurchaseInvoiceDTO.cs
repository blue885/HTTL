using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;

namespace MVCDTO.PurchaseTasks
{
    public class PurchaseInvoicePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.PurchaseInvoiceID; }
        public void SetID(int id) { this.PurchaseInvoiceID = id; }

        public int PurchaseInvoiceID { get; set; }

        [Display(Name = "Khách sạn")]
        public int HotelID { get; set; }

        [Display(Name = "Ngày mua")]
        public System.DateTime PurchaseInvoiceDate { get; set; }

        [Display(Name = "Số phiếu")]
        public string PurchaseInvoiceReference { get; set; }

        [Display(Name = "Nhà cung cấp")]
        [Required]
        public string SupplierName { get; set; }

        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }

        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }

        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }
    }

    public class PurchaseInvoiceDTO : PurchaseInvoicePrimitiveDTO, IBaseDetailEntity<PurchaseInvoiceDetailDTO>
    {
        public PurchaseInvoiceDTO()
        {
            this.PurchaseInvoiceDetails = new List<PurchaseInvoiceDetailDTO>();
        }


        public List<PurchaseInvoiceDetailDTO> PurchaseInvoiceDetails { get; set; }

        public ICollection<PurchaseInvoiceDetailDTO> GetDetails() { return this.PurchaseInvoiceDetails; }
    }
}
