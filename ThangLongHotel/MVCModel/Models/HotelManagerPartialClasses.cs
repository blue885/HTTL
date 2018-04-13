using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MVCModel.Models
{

    [MetadataType(typeof(BillingMasterMeta))]
    public partial class BillingMaster
    {
    }

    [MetadataType(typeof(ListRoomMeta))]
    public partial class ListRoom
    {
    }

    [MetadataType(typeof(HotelRoomMeta))]
    public partial class HotelRoom
    {
        public string PromptDescriptionInTitleCase
        {
            get
            {
                return this.PromptDescription == null ? "" : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.PromptDescription.ToLower());
            }
        }
    }

    [MetadataType(typeof(BillingDetailMeta))]
    public partial class BillingDetail
    {
    }

    [MetadataType(typeof(BillingDetailFullMeta))]
    public partial class BillingDetailFull
    {
    }



    public partial class HotelRoom
    {
        public string ArrivalDateFormated
        {
            get
            {
                if (this.ArrivalDate.HasValue)
                    return ((DateTime)this.ArrivalDate).ToString(this.RoomCategoryID == 1 ? "dd/MM/yyyy HH:mm" : "HH:mm");
                else return "";
            }
        }
    }
}
