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
    
    public partial class BillingList
    {
        public int BillingID { get; set; }
        public System.DateTime BillingDate { get; set; }
        public string BillingReference { get; set; }
        public int RoomID { get; set; }
        public string RoomDescription { get; set; }
        public int HotelID { get; set; }
        public string HotelDescription { get; set; }
        public System.DateTime ArrivalDate { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdentityNo { get; set; }
        public int ChargeTypeID { get; set; }
        public string ChargeTypeDescription { get; set; }
        public double ChargeDuration { get; set; }
        public double ChargeAmount { get; set; }
        public double ServiceAmount { get; set; }
        public double OtherAmount { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public string Remarks { get; set; }
    }
}
