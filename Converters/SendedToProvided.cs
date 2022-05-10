using PO;
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
    public class SendedToProvided : IValueConverter
    {
        /// <summary>
        /// check for a customer which parcels he almost got
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BlApi.IBL bl;
            BO.Customer customer = new();
            try
            {
                bl = BlApi.BlFactory.GetBl();
                lock (bl)
                {
                    customer = bl.RequestCustomer(new BO.Customer() { Id = (int)value });
                    if (customer.ToCustomer.Count > 0)
                    {
                        var ids = customer.ToCustomer.Select(c => c.Id);
                        var provided = ids.Where(id => bl.RequestParcel(new BO.Parcel() { Id = id }).Delivered != null).Select(id => $"Parcel ID: {id}");
                        return provided;
                    }
                }
                return new List<string>();
            }
            catch (BO.UnextantException)
            {
                return new List<string>();
            }
            catch (BO.XMLFileLoadCreateException)
            {
                return new List<string>();
            }
            catch (BO.DiscrepanciesException)
            {
                return new List<string>();
            }
            catch (BO.BLConfigException)
            {
                return new List<string>();
            }
        }
           
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
