using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListRoomTypeBuilder : IListRoomTypeBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForListRoomTypes(IEnumerable<ListRoomType> listRoomTypes)
        {
            return listRoomTypes.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.RoomTypeID.ToString() }).ToList();
        }
    }
}