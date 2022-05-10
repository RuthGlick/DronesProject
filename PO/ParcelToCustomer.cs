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
    public class ParcelToCustomer: DependencyObject
    {
        public static readonly DependencyProperty id =
          DependencyProperty.Register("Id", typeof(int), typeof(ParcelToCustomer), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty weight =
            DependencyProperty.Register("Weight", typeof(WeightCategories), typeof(ParcelToCustomer), new PropertyMetadata((WeightCategories.HEAVY)));
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(weight); }
            set { SetValue(weight, value); }
        }

        public static readonly DependencyProperty priority =
            DependencyProperty.Register("Priority", typeof(Priorities), typeof(ParcelToCustomer), new PropertyMetadata((Priorities.REGULAR)));
        public Priorities Priority
        {
            get { return (Priorities)GetValue(priority); }
            set { SetValue(priority, value); }
        }

        public static readonly DependencyProperty status =
          DependencyProperty.Register("Status", typeof(DroneStatuses), typeof(ParcelToCustomer), new PropertyMetadata(DroneStatuses.MAINTENANCE));
        public DroneStatuses Status
        {
            get { return (DroneStatuses)GetValue(status); }
            set { SetValue(status, value); }
        }

        public static readonly DependencyProperty partner =
            DependencyProperty.Register("Partner", typeof(CustomerInParcel), typeof(ParcelToCustomer), new PropertyMetadata(null));
        public CustomerInParcel Partner
        {
            get { return (CustomerInParcel)GetValue(partner); }
            set { SetValue(partner, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
       
        #endregion
    }
}

