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
using PL.Model;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListView.xaml
    /// </summary>
    public partial class DroneListView : Window
    {
        private BlApi.IBL bl;
        private int statusChosen = -1;
        private int weightChosen = -1;
        private bool buttonCancel = false;
        private PL.Model.DronesListModel droneModel;

        /// <summary>
        /// initialize components, the list of drones and 2 combo box of options which drones to show
        /// </summary>
        /// <param name="bl">first IBL type</param>
        public DroneListView(BlApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            droneModel = PL.Model.DronesListModel.Instance;
            droneModel.DroneView.Filter = FilterDrone;
            DataContext = droneModel;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BL.Enum.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BL.Enum.WeightCategories));
        }

        /// <summary>
        /// filter drone acording to Status and Weight
        /// </summary>
        /// <param name="obj">first object type</param>
        /// <returns>bool</returns>
        private bool FilterDrone(object obj)
        {
            if (obj is PO.DroneForList drone)
            {
                return (statusChosen == -1 || (int)drone.Status == statusChosen)
                    && (weightChosen == -1 || (int)drone.Weight == weightChosen);
            }
            return false;
        }

        /// <summary>
        /// show only part of the list of the drones according the request of the user
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second SelectionChangedEventArgs type</param>
        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source == StatusSelector)
            {
                statusChosen = (int)StatusSelector.SelectedItem;
            }
            else
            {
                weightChosen = (int)WeightSelector.SelectedItem;
            }
            droneModel.DroneView.Refresh();
        }

        /// <summary>
        /// show the AddDrone window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ButtonView(object sender, RoutedEventArgs e)
        {
            new DroneView(bl).Show();
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
        /// show the DroneView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Drone d = new BO.Drone();
            d.Id = ((PO.DroneForList)DronesListView.SelectedItem).Id;
            Drone temp = new Drone();
            try
            {
                temp = bl.RequestDrone(d);
            }
            catch (BO.UnextantException ex)
            {
                if (ex.Message == "drone")
                {
                    MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                }
                else
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                        $"the details of the chosen drone");
                }
            }
            catch (BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"failed to update the details of the chosen drone, {ex.Message}");
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            new DroneView(bl, temp).Show();
        }

        /// <summary>
        /// show the combobox which the user asked to see and collapse the second combobox
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void ButtonSelect(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == StatusSelectorButton)
            {
                StatusSelector.Visibility = Visibility.Visible;
                WeightSelector.Visibility = Visibility.Collapsed;
            }
            if (e.Source == WeightSelectorButton)
            {
                WeightSelector.Visibility = Visibility.Visible;
                StatusSelector.Visibility = Visibility.Collapsed;
            }
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
        /// cancels the Grouping of drones list and cancels the previous sort
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            statusChosen = -1;
            weightChosen = -1;
            droneModel.DroneView.GroupDescriptions.Clear();
                droneModel.RefreshDrones();
        }

        /// <summary>
        /// Group Drones list By Status
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void GroupBy(object sender, RoutedEventArgs e)
        {
            droneModel.DroneView.GroupDescriptions.Clear();
            droneModel.DroneView.GroupDescriptions.Add(new PropertyGroupDescription("Status"));
        }
    }
}
