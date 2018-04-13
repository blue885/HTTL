using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCCore.Repositories.CommonTasks
{ 

    public interface IListChargeTypeRepository : IGenericRepository<ListChargeType>
    {
        IList<ListChargeType> GetAllListChargeType();
    }
}
