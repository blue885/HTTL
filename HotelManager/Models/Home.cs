using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.Models
{
    public class Home
    {
        public static int GetHotelID(HttpContextBase context)
        {
            if (context.Session["HotelID"] == null)
                return 0;
            else
                return (int)context.Session["HotelID"];
        }

        public static void SetHotelID(HttpContextBase context, int hotelID)
        {
            context.Session["HotelID"] = hotelID;
        }
    }
}