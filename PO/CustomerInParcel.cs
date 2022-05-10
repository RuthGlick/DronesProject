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
    public class CustomerInParcel: DependencyObject
    {
        public static readonly DependencyProperty id =
           DependencyProperty.Register("Id", typeof(int), typeof(CustomerInParcel), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty name =
            DependencyProperty.Register("Name", typeof(string), typeof(CustomerInParcel), new PropertyMetadata(string.Empty));
        public string Name
        {
            get { return (string)GetValue(name); }
            set
            {
                SetValue(name, value);
            }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }

    }
}

