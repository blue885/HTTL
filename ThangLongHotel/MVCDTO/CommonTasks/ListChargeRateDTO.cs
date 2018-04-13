using MVCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListChargeRatePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.ChargeRateID; }
        public void SetID(int id) { this.ChargeRateID = id; }

        public int ChargeRateID { get; set; }
   
        public int RoomTypeID { get; set; }
        public string ListRoomTypeDescription { get; set; }
        public int ChargeTypeID { get; set; }
        public string ListChargeTypeDescription { get; set; }
        public int ChargeVolumn { get; set; }
        public double ChargeRate { get; set; }
        public double ChargeRateUpper { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
            
    }

    public class ListChargeRateDTO : ListChargeRatePrimitiveDTO
    {
        public ListChargeRateDTO()
        {
            
        }
    }   
}
