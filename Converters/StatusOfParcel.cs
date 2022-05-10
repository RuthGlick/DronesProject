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
    public class StatusOfParcel : IValueConverter
    {
        /// <summary>
        /// check the status of a parcel
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((int)value != 0)
            {
                BlApi.IBL bl;
                BO.Parcel parcel = new();
                try
                {
                    bl = BlApi.BlFactory.GetBl();
                    lock (bl)
                    {
                        parcel = bl.RequestParcel(new BO.Parcel() { Id = (int)value });
                    }
                }
                catch (BO.UnextantException)
                {
                    return "";
                }
                catch (BO.XMLFileLoadCreateException)
                {
                    return "";
                }
                catch (BO.DiscrepanciesException)
                {
                    return "";
                }
                catch (BO.BLConfigException)
                {
                    return "";
                }
                if (parcel.Delivered != null)
                {
                    return "Provided";
                }
                if (parcel.PickedUp != null)
                {
                    return "Picked Up";
                }
                if (parcel.Scheduled != null)
                {
                    return "Scheduled";
                }
                return "Created";
            }
            else
            {
                return "";
            }
        }
           
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
