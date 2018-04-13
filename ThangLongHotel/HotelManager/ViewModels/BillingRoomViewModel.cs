using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManager.ViewModels
{
    public class BillingRoomViewModel
    {
        public int HotelID { get; set; }
        public int RoomCategoryID { get; set; }

        public int BillingID { get; set; }

        [Display(Name = "Phòng")]
        public int RoomID { get; set; }
        

        [Display(Name = "Phòng chuyển đến")]
        [Required(ErrorMessage = "Vui lòng chọn phòng")]
        public int? NewRoomID { get; set; }

        [Display(Name = "Lý do chuyển")]
        [Required(ErrorMessage = "Vui lòng nhập lý do chuyển phòng")]
        public string Description { get; set; }
    }
}