using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders
{   
    public interface IHotelSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForHotels(IEnumerable<ListHotel> hotels);
    }

}