using HotelManager.ViewModels.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListServiceBuilder : IListServiceBuilder
    {
        public ListServiceBuilder()
        {
                
        }

        public void BuildSelectListsForListServiceViewModel(ListServiceViewModel listServiceViewModel)
        {
         
        }

        public IEnumerable<SelectListItem> BuildSelectListItemsForListServices(IEnumerable<ListService> listServices)
        {
            return listServices.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.ServiceID.ToString() }).ToList();
        }
    }
}