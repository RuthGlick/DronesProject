using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ValueConverterDemo
{
    public class BatteryToBrushConverter : IValueConverter
    {
        /// <summary>
        /// convert from double property to Brushes
        /// </summary>
        /// <param name="value">the first object value</param>
        /// <param name="targetType">the second Type value</param>
        /// <param name="parameter">the third object value</param>
        /// <param name="culture">the forth CultureInfo value</param>
        /// <returns>object value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((double)value)
            {
                case <10:
                    return Brushes.DarkRed;
                default:
                    return Brushes.ForestGreen;
            }
        }
           
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
