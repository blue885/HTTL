using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;

namespace MVCService.CommonTasks
{
    public class ListRoomTypeService : GenericService<ListRoomType, ListRoomTypeDTO, ListRoomTypePrimitiveDTO>, IListRoomTypeService
    {        

        private readonly IListRoomTypeRepository listRoomTypeRepository;

        public ListRoomTypeService(IListRoomTypeRepository listRoomTypeRepository)
            : base(listRoomTypeRepository)
        {
            this.listRoomTypeRepository = listRoomTypeRepository;
        }
  
    }
}
