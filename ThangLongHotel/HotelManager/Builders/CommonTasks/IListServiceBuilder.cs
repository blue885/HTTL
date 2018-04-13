using HotelManager.ViewModels.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public interface IListServiceBuilder
    {
        void BuildSelectListsForListServiceViewModel(ListServiceViewModel listServiceViewModel);
        IEnumerable<SelectListItem> BuildSelectListItemsForListServices(IEnumerable<ListService> listServices);
    }
}