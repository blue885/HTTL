using HotelManager.ViewModels.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public interface IListRoomBuilder
    {
        void BuildSelectListsForListRoomViewModel(ListRoomViewModel listRoomViewModel);
        //IEnumerable<SelectListItem> BuildSelectListItemsForListRooms(IEnumerable<ListRoom> listRoom);
    }
}