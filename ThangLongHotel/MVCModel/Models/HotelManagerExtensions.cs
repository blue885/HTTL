using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCModel.Models
{
    class HotelManagerExtensions
    {
    }

    public partial class PurchaseInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PurchaseInvoiceDetail> 
    { 
        public int GetID() { return this.PurchaseInvoiceID; }

        public ICollection<PurchaseInvoiceDetail> GetDetails() { return this.PurchaseInvoiceDetails; }
    }
    
    public partial class PurchaseInvoiceDetail : IPrimitiveEntity 
    { 
        public int GetID() { return this.PurchaseInvoiceDetailID; }
    }

    public partial class ListChargeRate : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.ChargeRateID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ListRoomType : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.RoomTypeID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ListChargeType : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.ChargeTypeID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ListRoom : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.RoomID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ListRoomStatuses : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.RoomStatusID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ListService : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.ServiceID; }

        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }
}
