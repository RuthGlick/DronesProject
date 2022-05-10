using PL;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL bl;

        /// <summary>
        /// Initialize the Component
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                bl = BlApi.BlFactory.GetBl();
            }
            catch (BO.BLConfigException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"failed to create a list of drones and update their details, {ex.Message}");
            }
            catch (BO.UnextantException ex)
            {
                MessageBox.Show($"{ ex.Message} was not found and it was needed to create a list of drones and update their details");
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method open the DroneListView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void DroneList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListView(bl).Show();
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method open the BaseStationListView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void BaseStationList_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListView(bl).Show();
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method open the CustomerListView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void CustomerList_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListView(bl).Show();
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method open the ParcelListView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ParcelList_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListView(bl).Show();
        }
    }
}
