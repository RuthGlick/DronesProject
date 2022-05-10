using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PO.Enum;


namespace PO
{
    public class ParcelInTransfer :DependencyObject, ILocate
    {
        public static readonly DependencyProperty id =
         DependencyProperty.Register("Id", typeof(int), typeof(ParcelInTransfer), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty status =
           DependencyProperty.Register("Status", typeof(bool), typeof(ParcelInTransfer), new PropertyMetadata(false));
        public bool Status
        {
            get { return (bool)GetValue(status); }
            set { SetValue(status, value); }
        }

        public static readonly DependencyProperty weight =
           DependencyProperty.Register("Weight", typeof(WeightCategories), typeof(ParcelInTransfer), new PropertyMetadata((WeightCategories.HEAVY)));
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(weight); }
            set { SetValue(weight, value); }
        }

        public static readonly DependencyProperty priority =
         DependencyProperty.Register("Priority", typeof(Priorities), typeof(ParcelInTransfer), new PropertyMetadata((Priorities.REGULAR)));
        public Priorities Priority
        {
            get { return (Priorities)GetValue(priority); }
            set { SetValue(priority, value); }
        }

        public static readonly DependencyProperty sender =
         DependencyProperty.Register("Sender", typeof(CustomerInParcel), typeof(ParcelInTransfer), new PropertyMetadata(null));
        public CustomerInParcel Sender
        {
            get { return (CustomerInParcel)GetValue(sender); }
            set { SetValue(sender, value); }
        }

        public static readonly DependencyProperty target =
       DependencyProperty.Register("Target", typeof(CustomerInParcel), typeof(ParcelInTransfer), new PropertyMetadata(null));
        public CustomerInParcel Target
        {
            get { return (CustomerInParcel)GetValue(target); }
            set { SetValue(target, value); }
        }

        public static readonly DependencyProperty location =
          DependencyProperty.Register("Location", typeof(Location), typeof(ParcelInTransfer), new PropertyMetadata(null));
        public Location Location
        {
            get { return (Location)GetValue(location); }
            set { SetValue(location, value); }
        }

        public static readonly DependencyProperty destination =
         DependencyProperty.Register("Destination", typeof(Location), typeof(ParcelInTransfer), new PropertyMetadata(null));
        public Location Destination
        {
            get { return (Location)GetValue(destination); }
            set { SetValue(destination, value); }
        }

        public static readonly DependencyProperty transportDistance =
         DependencyProperty.Register("TransportDistance", typeof(double), typeof(ParcelInTransfer), new PropertyMetadata(0.0));
        public double TransportDistance
        {
            get { return (double)GetValue(transportDistance); }
            set { SetValue(transportDistance, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public override string ToString()
        {
            return $"Id: {Id} sender-Id: {Sender} target-Id: {Target}";
        }
    }
}
