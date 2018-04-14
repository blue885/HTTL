using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCCore.Repositories.PurchaseTasks;
using MVCDTO.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCore.Repositories.CommonTasks;
using MVCDTO.CommonTasks;

namespace HotelManager.Api.CommonTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ListRoomsApiController : Controller
    {
        private readonly IListRoomRepository listRoomRepository;

        public ListRoomsApiController(IListRoomRepository listRoomRepository)
        {
            this.listRoomRepository = listRoomRepository;
        }

        public JsonResult GetListRooms([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ListRoom> listRooms = this.listRoomRepository.GetAll();

            DataSourceResult response = listRooms.ToDataSourceResult(request, o => new ListRoomPrimitiveDTO
            {
                RoomID = o.RoomID,
                RoomTypeID = o.RoomTypeID,
                HotelID = o.HotelID,
                ListHotelDescription = o.ListHotel.Description,
                ListRoomTypeDescription = o.ListRoomType.Description,
                RoomStatusID = o.RoomStatusID,
                ListRoomStatusDescription = o.ListRoomStatu.Description,
                FloorLevel = o.FloorLevel,
                NoBed = o.NoBed,
                NoPerson = o.NoPerson,                
                Description = o.Description,
                Remarks = o.Remarks 
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }       
    
    }
}