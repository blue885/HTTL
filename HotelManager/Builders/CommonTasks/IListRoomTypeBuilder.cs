using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public interface IListRoomTypeBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForListRoomTypes(IEnumerable<ListRoomType> listRoomTypes);
    }
}