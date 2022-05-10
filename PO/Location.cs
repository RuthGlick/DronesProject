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
    public class Location: DependencyObject
    {
        public static readonly DependencyProperty longitude =
        DependencyProperty.Register("Longitude", typeof(double), typeof(Location), new PropertyMetadata(0.0));
        public double Longitude
        {
            get { return (double)GetValue(longitude); }
            set { SetValue(longitude, value); }
        }

        public static readonly DependencyProperty latitude =
        DependencyProperty.Register("Latitude", typeof(double), typeof(Location), new PropertyMetadata(0.0));
        public double Latitude
        {
            get { return (double)GetValue(latitude); }
            set { SetValue(latitude, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
    
        #endregion

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"longitude: {Longitude}, latitude: {Latitude}";
        }
    }
}
