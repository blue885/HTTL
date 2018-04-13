using MVCModel;
using System;
using System.Collections.Generic;
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
        public string Description { get; set; }
        public string UnitForSales { get; set; }
        public double UnitPrice { get; set; }
        public string Remarks { get; set; }
    }

    public class ListServiceDTO : ListServicePrimitiveDTO
    {
        public ListServiceDTO()
        {
            
        }
    }   
}
