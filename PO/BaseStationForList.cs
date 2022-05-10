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
    public class BaseStationForList : DependencyObject
    {
        public static readonly DependencyProperty id =
         DependencyProperty.Register("Id", typeof(int), typeof(BaseStationForList), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty name =
          DependencyProperty.Register("Name", typeof(string), typeof(BaseStationForList), new PropertyMetadata(string.Empty));
        public string Name
        {
            get { return (string)GetValue(name); }
            set { SetValue(name, value); }
        }

        public static readonly DependencyProperty availableChargeSlots =
                   DependencyProperty.Register("AvailableChargeSlots", typeof(int), typeof(BaseStationForList), new PropertyMetadata(0));
        public int AvailableChargeSlots
        {
            get { return (int)GetValue(availableChargeSlots); }
            set { SetValue(availableChargeSlots, value); }
        }

        public static readonly DependencyProperty catchChargeSlots =
           DependencyProperty.Register("CatchChargeSlots", typeof(int), typeof(BaseStationForList), new PropertyMetadata(0));
        public int CatchChargeSlots
        {
            get { return (int)GetValue(catchChargeSlots); }
            set { SetValue(catchChargeSlots, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
      
        #endregion

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"baseStation to list - id: {Id}, name: {Name}, availableChargeSlots: {AvailableChargeSlots}, catchChargeSlots: {CatchChargeSlots}";
        }

    }
}

