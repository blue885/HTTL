using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListRoomStatusesService : GenericService<ListRoomStatuses, ListRoomStatusesDTO, ListRoomStatusesPrimitiveDTO>, IListRoomStatusesService
    {        

        private readonly IListRoomStatusesRepository listRoomStatusesRepository;

        public ListRoomStatusesService(IListRoomStatusesRepository listRoomStatusesRepository)
            : base(listRoomStatusesRepository)
        {
            this.listRoomStatusesRepository = listRoomStatusesRepository;
        }
  
    }
}
