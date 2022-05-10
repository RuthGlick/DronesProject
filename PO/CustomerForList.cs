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
    public class CustomerForList : DependencyObject
    {
        public static readonly DependencyProperty id =
            DependencyProperty.Register("Id", typeof(int), typeof(CustomerForList), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty name =
           DependencyProperty.Register("Name", typeof(string), typeof(CustomerForList), new PropertyMetadata(string.Empty));
        public string Name
        {
            get { return (string)GetValue(name); }
            set
            {
                SetValue(name, value);
            }
        }


        public static readonly DependencyProperty phoneNumber =
           DependencyProperty.Register("PhoneNumber", typeof(string), typeof(CustomerForList), new PropertyMetadata(string.Empty));
        public string PhoneNumber
        {
            get { return (string)GetValue(phoneNumber); }
            set { SetValue(phoneNumber, value); }
        }

        public static readonly DependencyProperty deliveredPackages =
         DependencyProperty.Register("DeliveredPackages", typeof(int), typeof(CustomerForList), new PropertyMetadata(0));
        public int DeliveredPackages
        {
            get { return (int)GetValue(deliveredPackages); }
            set { SetValue(deliveredPackages, value); }
        }

        public static readonly DependencyProperty sendedPackages =
       DependencyProperty.Register("SendedPackages", typeof(int), typeof(CustomerForList), new PropertyMetadata(0));
        public int SendedPackages
        {
            get { return (int)GetValue(sendedPackages); }
            set { SetValue(sendedPackages, value); }
        }

        public static readonly DependencyProperty acceptedPackages =
    DependencyProperty.Register("AcceptedPackages", typeof(int), typeof(CustomerForList), new PropertyMetadata(0));
        public int AcceptedPackages
        {
            get { return (int)GetValue(acceptedPackages); }
            set { SetValue(acceptedPackages, value); }
        }

        public static readonly DependencyProperty packagesInWay =
DependencyProperty.Register("PackagesInWay", typeof(int), typeof(CustomerForList), new PropertyMetadata(0));
        public int PackagesInWay
        {
            get { return (int)GetValue(packagesInWay); }
            set { SetValue(packagesInWay, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
      
        #endregion

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"customer in list - id: {Id}, name: {Name}, phoneNumber: {PhoneNumber}" +
                $"delivered packages: {DeliveredPackages}, sended packages: {SendedPackages}, " +
                $"accepted packages: {AcceptedPackages}, packages in way: {PackagesInWay}";
        }

    }
}