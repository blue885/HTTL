using MVCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListChargeTypePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.ChargeTypeID; }
        public void SetID(int id) { this.ChargeTypeID = id; }

        public int ChargeTypeID { get; set; }
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        public double HoursPerBlock { get; set; }
        public double GraceMinutes { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }

    }

    public class ListChargeTypeDTO : ListChargeTypePrimitiveDTO
    {
        public ListChargeTypeDTO()
        {

        }
    }       
       
}
