using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListView.xaml
    /// </summary>
    public partial class CustomerListView : Window
    {
        private BlApi.IBL bl;
        private bool buttonCancel = false;
        private PL.Model.CustomersListModel customersModel;

        /// <summary>
        /// initialize components,and the list of customers
        /// </summary>
        /// <param name="bl">first IBL type</param>
        public CustomerListView(BlApi.IBL bl)
        {
            this.bl = bl;
            InitializeComponent();
            customersModel = PL.Model.CustomersListModel.Instance;
            DataContext = customersModel;
        }

        /// <summary>
        /// show the AddCustomer window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void CustomerView(object sender, RoutedEventArgs e)
        {
            new CustomerView(bl).Show();
        }

        /// <summary>
        /// close this window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            buttonCancel = true;
            this.Close();
            buttonCancel = false;
        }

        /// <summary>
        /// show the CustomerView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new CustomerView(bl, ((PO.CustomerForList)CustomersListView.SelectedItem)).Show();
        }

        /// <summary>
        /// if buttonCancel = false the function does not close this window, else - it closes
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn CancelEventArgs type</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (buttonCancel == false)
            {
                e.Cancel = true;
            }
        }
    }
}
