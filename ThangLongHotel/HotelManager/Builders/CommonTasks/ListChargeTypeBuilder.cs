using HotelManager.ViewModels.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListChargeTypeBuilder : IListChargeTypeBuilder
    {        

        public ListChargeTypeBuilder()
        {
                
        }

        public void BuildSelectListsForListChargeTypeViewModel(ListChargeTypeViewModel listChargeTypeViewModel)
        {
            
        }

        public IEnumerable<SelectListItem> BuildSelectListItemsForListChargeTypes(IEnumerable<ListChargeType> listChargeTypes)
        {
            return listChargeTypes.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.ChargeTypeID.ToString() }).ToList();
        }
    }
}