using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace PO
{
    public class BaseStation : DependencyObject, ILocate
    {
        public static readonly DependencyProperty id =
         DependencyProperty.Register("Id", typeof(int), typeof(BaseStation), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty name =
         DependencyProperty.Register("Name", typeof(string), typeof(BaseStation), new PropertyMetadata(string.Empty));
        public string Name
        {
            get { return (string)GetValue(name); }
            set { SetValue(name, value); }
        }

        public static readonly DependencyProperty location =
         DependencyProperty.Register("Location", typeof(Location), typeof(BaseStation), new PropertyMetadata(null));
        public Location Location
        {
            get { return (Location)GetValue(location); }
            set { SetValue(location, value); }
        }

        public static readonly DependencyProperty availableChargeSlots =
            DependencyProperty.Register("AvailableChargeSlots", typeof(int), typeof(BaseStation), new PropertyMetadata());
        public int AvailableChargeSlots
        {
            get { return (int)GetValue(availableChargeSlots); }
            set { SetValue(availableChargeSlots, value); }
        }

        public static readonly DependencyProperty oDronesInCharging =
            DependencyProperty.Register("ODronesInCharging", typeof(List<DroneInCharging>), typeof(BaseStation), new PropertyMetadata(new List<DroneInCharging>()));
        public List<DroneInCharging> ODronesInCharging
        {
            get { return (List<DroneInCharging>)GetValue(oDronesInCharging); }
            set { SetValue(oDronesInCharging, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

