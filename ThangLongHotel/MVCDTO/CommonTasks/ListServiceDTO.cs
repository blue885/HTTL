using MVCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListServicePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.ServiceID; }
        public void SetID(int id) { this.ServiceID = id; }

        public int ServiceID { get; set; }
        [Display(Name = "Tên DV/Hàng hóa")]
        public string Description { get; set; }
        [Display(Name = "Đơn vị tính")]
        public string UnitForSales { get; set; }
        [Display(Name = "Đơn giá")]
        public double UnitPrice { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }
    }

    public class ListServiceDTO : ListServicePrimitiveDTO
    {
        public ListServiceDTO()
        {
            
        }
    }   
}
