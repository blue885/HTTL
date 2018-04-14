using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HotelManager.Configuration
{
    public static class SettingsManager
    {
        public static int AutoCompleteMinLenght = 2;
        public static string DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        public static string MonthDay = "dd/MM";
        //public static string MonthDay = MonthDayFormat();
        public static string DeleteAlert = "Are you sure you want to delete this record?";

        public static int RoomTypeTable = 5;
        public static int RoomCategoryRoomService = 1;

        private static string MonthDayFormat()
        {
            string shortPattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            while (shortPattern[0] != 'd' && shortPattern[0] != 'M')
            {
                shortPattern = shortPattern.Substring(1);
                if (shortPattern.Length == 0)
                    return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
            while (shortPattern[shortPattern.Length - 1] != 'd' && shortPattern[shortPattern.Length - 1] != 'M')
            {
                shortPattern = shortPattern.Substring(0, shortPattern.Length - 1);
                if (shortPattern.Length == 0)
                    return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
            return shortPattern;
        }
       
    }       
}