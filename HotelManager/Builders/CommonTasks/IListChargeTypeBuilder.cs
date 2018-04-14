using HotelManager.ViewModels.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public interface IListChargeTypeBuilder
    {
        void BuildSelectListsForListChargeTypeViewModel(ListChargeTypeViewModel listChargeTypeViewModel);
        IEnumerable<SelectListItem> BuildSelectListItemsForListChargeTypes(IEnumerable<ListChargeType> listChargeTypes);
    }
}