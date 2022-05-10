using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterDemo
{
    public class StatusConvert2 : IValueConverter
    {
        /// <summary>
        /// the function convert "Status: AVAILABLE" to "send to transport" and  "Status: TRANSPORT" to "supply"
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PO.Enum.DroneStatuses)value == (PO.Enum.DroneStatuses)0)
            {
                return "send to transport";
            }
            else
            {
                if ((PO.Enum.DroneStatuses)value == (PO.Enum.DroneStatuses)2)
                {
                    return "supply";
                }
                else
                {
                    return "";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}