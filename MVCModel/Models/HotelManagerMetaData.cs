using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCModel.Models
{
    public class BillingMasterMeta
    {

        [Display(Name = "Phòng")]
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Billing Reference is required.")]
        public string BillingReference { get; set; }

        [Display(Name = "Giờ nhận phòng")]
        [Required(ErrorMessage = "Vui lòng nhập ngày giờ nhận phòng")]
        public System.DateTime ArrivalDate { get; set; }

        [Display(Name = "Giờ trả phòng")]
        [Required(ErrorMessage = "Vui lòng nhập ngày giờ trả phòng")]
        //[GenericCompare(CompareToPropertyName = "ArrivalDate", OperatorName = GenericCompareOperator.GreaterThanOrEqual, ErrorMessage = "Ngày trả phòng không được nhỏ hơn ngày nhận phòng")]
        public System.DateTime DepartureDate { get; set; }

        [Display(Name = "Tên khách thuê phòng")]
        public string CustomerName { get; set; }

        [Display(Name = "CMND/ Passport")]
        [Required(ErrorMessage = "Vui lòng nhập CMND khách thuê phòng")]
        public string CustomerIdentityNo { get; set; }
        [Display(Name = "Giới tính (nữ)")]
        public bool CustomerSex { get; set; }
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
        [Display(Name = "Điện thoại")]
        public string CustomerTelephone { get; set; }
        [Display(Name = "Quốc tịch")]
        public string CustomerNationality { get; set; }

        [Display(Name = "Thuê theo giờ/ ngày")]
        public int ChargeTypeID { get; set; }
        [Display(Name = "Thời gian thuê")]
        public double ChargeDuration { get; set; }
        [Display(Name = "Tiền thuê phòng")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double ChargeAmount { get; set; }
        [Display(Name = "Tổng cộng tiền dịch vụ")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double ServiceAmount { get; set; }
        [Display(Name = "Tiền dịch vụ khác")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double OtherAmount { get; set; }
        [Display(Name = "% giảm giá")]
        public double DiscountPercent { get; set; }
        [Display(Name = "TC tiền giảm giá")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double DiscountAmount { get; set; }
        [Display(Name = "Tổng tiền phải thanh toán")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double TotalAmount { get; set; }
        [Display(Name = "Thanh toán trước")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public double AdvancePayment { get; set; }
        [Display(Name = "Thanh toán bằng thẻ")]
        public bool IsPaidByATMCard { get; set; }
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }
    }


    public class HotelRoomMeta
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM HH:mm}")]
        public Nullable<System.DateTime> ArrivalDate { get; set; }
    }

    public class ListRoomMeta
    {
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50)]
        public string Description { get; set; }
    }

    public class BillingDetailMeta
    {
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double Amount { get; set; }

    }

    public class BillingDetailFullMeta
    {
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public double Amount { get; set; }

    }
}
