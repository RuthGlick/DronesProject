using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterDemo
{
    public class VisibilityParcel : IValueConverter
    {
        /// <summary>
        /// convert from text to visible - if the text does not exist, the visibility is collapsed
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value+""=="")
            {
                return "Collapsed";
            }
            else
            {
                return "Visible";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

