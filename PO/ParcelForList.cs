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
    public class ParcelForList : DependencyObject
    {
        public static readonly DependencyProperty id =
          DependencyProperty.Register("Id", typeof(int), typeof(ParcelForList), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty senderName =
         DependencyProperty.Register("SenderName", typeof(string), typeof(ParcelForList), new PropertyMetadata(string.Empty));
        public string SenderName
        {
            get { return (string)GetValue(senderName); }
            set
            {
                SetValue(senderName, value);
            }
        }

        public static readonly DependencyProperty targetName =
         DependencyProperty.Register("TargetName", typeof(string), typeof(ParcelForList), new PropertyMetadata(string.Empty));
        public string TargetName
        {
            get { return (string)GetValue(targetName); }
            set
            {
                SetValue(targetName, value);
            }
        }

        public static readonly DependencyProperty weight =
             DependencyProperty.Register("Weight", typeof(WeightCategories), typeof(ParcelForList), new PropertyMetadata((WeightCategories.HEAVY)));
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(weight); }
            set { SetValue(weight, value); }
        }

        public static readonly DependencyProperty priority =
         DependencyProperty.Register("Priority", typeof(Priorities), typeof(ParcelForList), new PropertyMetadata((Priorities.REGULAR)));
        public Priorities Priority
        {
            get { return (Priorities)GetValue(priority); }
            set { SetValue(priority, value); }
        }

        public static readonly DependencyProperty status =
       DependencyProperty.Register("Status", typeof(DeliveryStatus), typeof(ParcelForList), new PropertyMetadata((DeliveryStatus.CREATED)));
        public DeliveryStatus Status
        {
            get { return (DeliveryStatus)GetValue(status); }
            set { SetValue(status, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
      
        #endregion

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"parcel to list -  id: {Id}, sender: {SenderName}, target: {TargetName}" +
                $"weight: {Weight}, priority: {Priority} status: {Status}";
        }

    }
}