using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterDemo
{
    public class StatusConvert1 : IValueConverter
    {
        /// <summary>
        /// the function convert "Status: AVAILABLE" to "charge drone" and  "Status: MAINTENANCE" to "release from charging" and "Status: TRANSPORT" to "pick up a parcel"
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
                return "charge drone";
            }
            else
            {
                if ((PO.Enum.DroneStatuses)value == (PO.Enum.DroneStatuses)1)
                {
                    return "release from charging";
                }
                else
                {
                    return "pick up a parcel";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
