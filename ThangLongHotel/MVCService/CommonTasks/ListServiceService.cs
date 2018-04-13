using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListServiceService : GenericService<ListService, ListServiceDTO, ListServicePrimitiveDTO>, IListServiceService
    {        

        private readonly IListServiceRepository listServiceRepository;

        public ListServiceService(IListServiceRepository listServiceRepository)
            : base(listServiceRepository)
        {
            this.listServiceRepository = listServiceRepository;
        }
  
    }
}
