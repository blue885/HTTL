//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCModel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseInvoiceDetail
    {
        public int PurchaseInvoiceDetailID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public int ServiceID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
    
        public virtual ListService ListService { get; set; }
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
    }
}