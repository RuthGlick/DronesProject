using BlApi;
using PO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterDemo
{
    public class DistanceFromDroneToParcel : IValueConverter
    {
        /// <summary>
        /// calculate the distance between a drone to his current destination
        /// </summary>
        /// <param name="value">first object </param>
        /// <param name="targetType">second Type </param>
        /// <param name="parameter">3yth object</param>
        /// <param name="culture">4th CultureInfo</param>
        /// <returns>object type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IBL bl;
            BO.Drone drone = new BO.Drone();
            try
            {
                bl = BlApi.BlFactory.GetBl();
                lock (bl)
                {
                    drone = bl.RequestDrone(new BO.Drone() { Id = (int)value });
                    if (drone.DeliveryByTransfer != null && drone.DeliveryByTransfer.Id != 0)
                    {
                        PO.Drone d = new PO.Drone() { Location = new PO.Location() { Longitude = drone.Location.Longitude, Latitude = drone.Location.Latitude } };
                        BO.Parcel p = bl.RequestParcel(new BO.Parcel() { Id = drone.DeliveryByTransfer.Id });
                        int id = p.PickedUp != null ? p.Target.Id : p.Sender.Id;
                        BO.Customer c = bl.RequestCustomer(new BO.Customer() { Id = id });
                        double distance = d.DistanceTo(new PO.Customer() { Location = new Location() { Longitude = c.Location.Longitude, Latitude = c.Location.Latitude } });
                        return "" + distance;
                    }
                }
                return "";
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
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

