using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders
{
    public class HotelSelectListBuilder : IHotelSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForHotels(IEnumerable<ListHotel> hotels)
        {
            return hotels.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.HotelID.ToString() }).ToList();
        }
    }
}