using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterDemo
{
    public class BackgroundConverter1 : IValueConverter
    {
        private Regex regexNumbers = new Regex("[^0-9][^.][^0-9]+");

        /// <summary>
        /// the function convert string param, valid string to "LightGray" and not to "DarkRed"
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != null && regexNumbers.IsMatch((string)value) == true)
            {
                return "DarkRed";
            }
            return "LightGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}