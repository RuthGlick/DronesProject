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
    /// Interaction logic for BaseStationListView.xaml
    /// </summary>
    public partial class BaseStationListView : Window
    {
        private BlApi.IBL bl;
        private bool buttonCancel = false;
        private PL.Model.BaseStationsListModel baseStationModel;

        /// <summary>
        /// initialize components and the list of base stations 
        /// </summary>
        /// <param name="bl">first IBL type</param>
        public BaseStationListView(BlApi.IBL bl)
        {
            this.bl = bl;
            InitializeComponent();
            baseStationModel = PL.Model.BaseStationsListModel.Instance;
            DataContext = baseStationModel;
        }

        /// <summary>
        /// show the AddBaseStation window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ButtonView(object sender, RoutedEventArgs e)
        {
            new BaseStationView(bl).Show();
        }

        /// <summary>
        /// close this window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            buttonCancel = true;
            this.Close();
            buttonCancel = false;
        }

        /// <summary>
        /// show the BaseStationView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void BaseStationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PO.BaseStationForList item = (PO.BaseStationForList)BaseStationsListView.SelectedItem;
            new BaseStationView(bl, item).Show();
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

        /// <summary>
        /// Group Base Stations list By AvailableChargeSlots
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void GroupBy(object sender, RoutedEventArgs e)
        {
            baseStationModel.BaseStationView.GroupDescriptions.Clear();
            baseStationModel.BaseStationView.GroupDescriptions.Add(new PropertyGroupDescription("AvailableChargeSlots"));
        }

        /// <summary>
        /// cancel the Grouping of Base Stations list
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            baseStationModel.BaseStationView.GroupDescriptions.Clear();
            try
            {
                baseStationModel.RefreShBaseStations();
            }
            catch(XMLFileLoadCreateException ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
