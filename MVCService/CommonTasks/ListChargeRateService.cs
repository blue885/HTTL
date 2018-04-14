using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListChargeRateService : GenericService<ListChargeRate, ListChargeRateDTO, ListChargeRatePrimitiveDTO>, IListChargeRateService
    {        

        private readonly IListChargeRateRepository listChargeRateRepository;

        public ListChargeRateService(IListChargeRateRepository listChargeRateRepository)
            : base(listChargeRateRepository)
        {
            this.listChargeRateRepository = listChargeRateRepository;
        }
  
    }
}
