using MVCModel;
using System;
using System.Collections.Generic;
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
        public string Description { get; set; }
        public double HoursPerBlock { get; set; }
        public double GraceMinutes { get; set; }
        public string Remarks { get; set; }
      
    }

    public class ListChargeTypeDTO : ListChargeTypePrimitiveDTO
    {
        public ListChargeTypeDTO()
        {

        }
    }       
       
}
