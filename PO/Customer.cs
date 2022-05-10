using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ValueConverterDemo;

namespace PO
{
    public class Customer : DependencyObject, ILocate
    {

        public static readonly DependencyProperty id =
           DependencyProperty.Register("Id", typeof(int), typeof(Customer), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }


        public static readonly DependencyProperty name =
           DependencyProperty.Register("Name", typeof(string), typeof(Customer), new PropertyMetadata(string.Empty));
        public string Name
        {
            get { return (string)GetValue(name); }
            set
            {
                SetValue(name, value);
            }
        }


        public static readonly DependencyProperty phoneNumber =
           DependencyProperty.Register("PhoneNumber", typeof(string), typeof(Customer), new PropertyMetadata(string.Empty));
        public string PhoneNumber
        {
            get { return (string)GetValue(phoneNumber); }
            set { SetValue(phoneNumber, value); }
        }

        public static readonly DependencyProperty location =
       DependencyProperty.Register("Location", typeof(Location), typeof(Customer), new PropertyMetadata(null));
        public Location Location
        {
            get { return (Location)GetValue(location); }
            set { SetValue(location, value); }
        }

        ObservableCollection<ParcelToCustomer> fromCustomer = new ObservableCollection<ParcelToCustomer>();
        public ObservableCollection<ParcelToCustomer> ofromCustomer
        {
            get => fromCustomer;
            set => fromCustomer = value;
        }

        ObservableCollection<ParcelToCustomer> toCustomer = new ObservableCollection<ParcelToCustomer>();
        public ObservableCollection<ParcelToCustomer> otoCustomer
        {
            get => toCustomer;
            set => toCustomer = value;
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

