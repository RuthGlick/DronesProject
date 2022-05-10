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
    public class Parcel : DependencyObject
    {
        public static readonly DependencyProperty id =
         DependencyProperty.Register("Id", typeof(int), typeof(Parcel), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty sender =
               DependencyProperty.Register("Sender", typeof(CustomerInParcel), typeof(Parcel), new PropertyMetadata(null));
        public CustomerInParcel Sender
        {
            get { return (CustomerInParcel)GetValue(sender); }
            set { SetValue(sender, value); }
        }

        public static readonly DependencyProperty target =
                 DependencyProperty.Register("TargetName", typeof(CustomerInParcel), typeof(Parcel), new PropertyMetadata(null));
        public CustomerInParcel Target
        {
            get { return (CustomerInParcel)GetValue(target); }
            set { SetValue(target, value); }
        }

        public static readonly DependencyProperty weight =
         DependencyProperty.Register("Weight", typeof(WeightCategories), typeof(Parcel), new PropertyMetadata((WeightCategories.HEAVY)));
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(weight); }
            set { SetValue(weight, value); }
        }

        public static readonly DependencyProperty priority =
         DependencyProperty.Register("Priority", typeof(Priorities), typeof(Parcel), new PropertyMetadata((Priorities.REGULAR)));
        public Priorities Priority
        {
            get { return (Priorities)GetValue(priority); }
            set { SetValue(priority, value); }
        }

        public static readonly DependencyProperty droneInParcel =
                DependencyProperty.Register("DroneInParcel", typeof(DroneInParcel), typeof(Parcel), new PropertyMetadata(null));
        public DroneInParcel DroneInParcel
        {
            get { return (DroneInParcel)GetValue(droneInParcel); }
            set { SetValue(droneInParcel, value); }
        }

        public static readonly DependencyProperty created =
               DependencyProperty.Register("Created", typeof(DateTime?), typeof(Parcel), new PropertyMetadata(new DateTime()));
        public DateTime? Created
        {
            get { return (DateTime?)GetValue(created); }
            set { SetValue(created, value); }
        }

        public static readonly DependencyProperty scheduled =
               DependencyProperty.Register("Scheduled", typeof(DateTime?), typeof(Parcel), new PropertyMetadata(new DateTime()));
        public DateTime? Scheduled
        {
            get { return (DateTime?)GetValue(scheduled); }
            set { SetValue(scheduled, value); }
        }

        public static readonly DependencyProperty pickedUp =
               DependencyProperty.Register("PickedUp", typeof(DateTime?), typeof(Parcel), new PropertyMetadata(new DateTime()));
        public DateTime? PickedUp
        {
            get { return (DateTime?)GetValue(pickedUp); }
            set { SetValue(pickedUp, value); }
        }

        public static readonly DependencyProperty delivered =
           DependencyProperty.Register("Delivered", typeof(DateTime?), typeof(Parcel), new PropertyMetadata(new DateTime()));
        public DateTime? Delivered
        {
            get { return (DateTime?)GetValue(delivered); }
            set { SetValue(delivered, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
