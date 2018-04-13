using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListRoomService : GenericService<ListRoom, ListRoomDTO, ListRoomPrimitiveDTO>, IListRoomService
    {        

        private readonly IListRoomRepository listRoomRepository;

        public ListRoomService(IListRoomRepository listRoomRepository)
            : base(listRoomRepository)
        {
            this.listRoomRepository = listRoomRepository;
        }
  
    }
}
