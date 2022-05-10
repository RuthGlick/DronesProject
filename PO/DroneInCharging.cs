using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PO
{
    public class DroneInCharging: DependencyObject
    {
        public static readonly DependencyProperty id =
          DependencyProperty.Register("Id", typeof(int), typeof(DroneInCharging), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty batteryStatus =
       DependencyProperty.Register("BatteryStatus", typeof(double), typeof(DroneInCharging), new PropertyMetadata(0.0));
        public double BatteryStatus
        {
            get { return (double)GetValue(batteryStatus); }
            set { SetValue(batteryStatus, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}

