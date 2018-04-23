using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManager.ViewModels
{
    public class DeleteBillingViewModel
    {      
        [Display(Name = "Ngày xóa")]
        [Required(ErrorMessage = "Vui lòng chọn ngày xóa")]
        public DateTime DeleteDate { get; set; }
        public string ErrorMessage { get; set; }
    }
}