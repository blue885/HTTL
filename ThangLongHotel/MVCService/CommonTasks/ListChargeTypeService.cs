using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListChargeTypeService : GenericService<ListChargeType, ListChargeTypeDTO, ListChargeTypePrimitiveDTO>, IListChargeTypeService
    {        

        private readonly IListChargeTypeRepository listChargeTypeRepository;

        public ListChargeTypeService(IListChargeTypeRepository listChargeTypeRepository)
            : base(listChargeTypeRepository)
        {
            this.listChargeTypeRepository = listChargeTypeRepository;
        }
  
    }
}
