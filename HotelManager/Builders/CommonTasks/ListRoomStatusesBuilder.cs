﻿using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListRoomStatusesBuilder : IListRoomStatusesBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForListRoomStatusess(IEnumerable<ListRoomStatuses> listRoomStatusess)
        {
            return listRoomStatusess.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.RoomStatusID.ToString() }).ToList();
        }
    }
}