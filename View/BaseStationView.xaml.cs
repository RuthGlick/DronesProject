using PL.Model;
using PO;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationView.xaml
    /// </summary>
    public partial class BaseStationView : Window
    {
        private BlApi.IBL bl;
        BaseStationModel model = new BaseStationModel();
        private Action refreshBaseStations;
        private bool buttonCancel = false;

        /// <summary>
        /// Initialize the Components initialize the current base station, initialize the details and the action predicate
        /// </summary>
        /// <param name="bl">first IBL.IBL object</param>
        /// <param name="drone">second BO.BaseStationForList object</param>
        /// <param name="initialize">3th Action predicate</param>
        public BaseStationView(BlApi.IBL bl, PO.BaseStationForList station)
        {
            BaseStation baseStation;
            model.IsAdd = false;
            this.bl = bl;
            InitializeComponent();
            refreshBaseStations = BaseStationsListModel.Instance.RefreShBaseStations;
            BO.BaseStation blBaseStation = new BO.BaseStation();
            blBaseStation.Id = station.Id;
            try
            {
                lock (bl)
                {
                    blBaseStation = bl.RequestBaseStation(blBaseStation);
                }
            }
            catch(BO.UnextantException ex)
            {
                MessageBox.Show($"the {ex.Message} is not exist in the data system");
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            baseStation = new BaseStation()
            {
                Id = blBaseStation.Id,
                Name = blBaseStation.Name,
                Location = new Location { Longitude = blBaseStation.Location.Longitude, Latitude = blBaseStation.Location.Latitude },
                AvailableChargeSlots = blBaseStation.AvailableChargeSlots
            };
            List<DroneInCharging> droneInCharging = (from drone in blBaseStation.DronesInCharching
                                                        select new DroneInCharging()
                                                    { BatteryStatus = drone.BatteryStatus, Id = drone.Id }).ToList();
            baseStation.ODronesInCharging = droneInCharging;
            model = model.GetBasseStationModel(baseStation);
            this.DataContext = model;
            Refresh.RefreshBaseStation = initBaseStation;
        }

        /// <summary>
        /// initialize components and values of text boxes
        /// so that the user will be able to fill the details of the base station
        /// </summary>
        /// <param name="bl">first IBL object</param>
        /// <param name="initilalizeDrones">second Action delegate</param>
        public BaseStationView(BlApi.IBL bl)
        {
            model.IsAdd = true;
            this.bl = bl;
            refreshBaseStations = BaseStationsListModel.Instance.RefreShBaseStations;
            InitializeComponent();
            init(textID, "baseStation id (5 characters)");
            init(textName, "base station name");
            init(textAvailableChargeSlots, "amount of available charge slots");
            this.DataContext = model;
        }

        /// <summary>
        /// init a textbox chief
        /// </summary>
        /// <param name="textBox">first textbox chief</param>
        /// <param name="str">second string arg</param>
        private void init(TextBox textBox, string str)
        {
            textBox.Foreground = Brushes.Gray;
            textBox.Text = str;
            textBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(text_GotKeyboardFocus);
            textBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(text_LostKeyboardFocus);
        }

        /// <summary>
        /// erase the text of a textbox chief when the focus on it 
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second KeyboardFocusChangedEventArgs type</param>
        private void text_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing has been entered yet.
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        /// <summary>
        /// when the focus lost from textbox chief the function change the text to be 
        /// the text which tells the user what to fill in gray.
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second KeyboardFocusChangedEventArgs type</param>
        private void text_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Make sure sender is the correct Control.
            if (sender is TextBox)
            {
                //If nothing was entered, reset default text.
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    if (sender == textName)
                    {
                        ((TextBox)sender).Text = "base Station name";
                    }
                    if (sender == textID)
                    {
                        ((TextBox)sender).Text = "baseStation id (5 characters)";
                    }
                    if (sender == textAvailableChargeSlots)
                    {
                        ((TextBox)sender).Text = "amount of available charge slots";
                    }
                }
            }
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
        /// add base station to the base stations collection
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Add(object sender, RoutedEventArgs e)
        {
            if (checkValid() == true)
            {
                BO.BaseStation b = new BO.BaseStation();
                b.Id = int.Parse(textID.Text);
                b.Name = textName.Text;
                b.AvailableChargeSlots = int.Parse(textAvailableChargeSlots.Text);
                b.Location.Longitude = double.Parse(textLongitude.Text);
                b.Location.Latitude = double.Parse(textLatitude.Text);
                try
                {
                    lock (bl)
                    {
                        bl.CreateBaseStation(b);
                    }
                    if (MessageBox.Show("the base station was seccessfully added", "success", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                    }
                    refreshBaseStations();
                }
                catch (BO.ExtantException ex)
                {
                    MessageBox.Show($"the {ex.Message} is almost exist in the data system");
                }
                catch(BO.IncorrectInputException ex)
                {
                    MessageBox.Show($"not valid input, {ex.Message}");
                }
                catch(BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            }
        }

        /// <summary>
        /// check if the details which the customer entered are in valid format
        /// </summary>
        /// <returns>bool</returns>
        private bool checkValid()
        {
            if (textID.Text == "" || textID.Text == "baseStation id (5 characters)" || textName.Text == "" || textName.Text == "customer name"
            || textLongitude.Text == "" || textLatitude.Text == "" || textAvailableChargeSlots.Text == "" || textAvailableChargeSlots.Text == "available charge slots")
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("All fields are required", "warning", button, icon);
                return false;
            }
            else
            {
                if (textID.Background == Brushes.DarkRed || textID.Text.Length != 5 || textID.Text=="00000")
                {
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show("not valid id", "warning", button, icon);
                    return false;
                }
                else
                {
                    if (textAvailableChargeSlots.Background == Brushes.DarkRed || int.Parse(textAvailableChargeSlots.Text) < 0)
                    {
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBox.Show("not valid num of available charge slots", "warning", button, icon);
                        return false;
                    }
                    else
                    {
                        if (textLongitude.Background == Brushes.DarkRed || textLatitude.Background == Brushes.DarkRed
                            || double.Parse(textLatitude.Text) < 0 || double.Parse(textLongitude.Text) < 0 || double.Parse(textLatitude.Text) > 180 || double.Parse(textLongitude.Text) > 180)
                        {
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBox.Show("not valid location", "warning", button, icon);
                            return false;
                        }
                    }
                }
            }
            return true;
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
        /// Enable to update
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn KeyboardFocusChangedEventArgs type</param>
        private void UpdateBaseStation(object sender, KeyboardFocusChangedEventArgs e)
        {
            update.IsEnabled = true;
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method update the model of base station
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            if (Name.Background == Brushes.DarkRed || AvailableChargeSlots.Background == Brushes.DarkRed)
            {
                MessageBox.Show("not valid input");
            }
            else
            {
                if (Name.Text == "" && AvailableChargeSlots.Text == "")
                {
                    MessageBox.Show("no details to update");
                }
                else
                {
                    BO.BaseStation b = new BO.BaseStation();
                    b.Id = model.MyBaseStation.Id;
                    if (Name.Text != "")
                        b.Name = Name.Text;
                    if (AvailableChargeSlots.Text != "")
                        b.AvailableChargeSlots = int.Parse(AvailableChargeSlots.Text);
                    try
                    {
                        lock (bl)
                        {
                            bl.UpdateBaseStation(b, b.AvailableChargeSlots + model.MyBaseStation.ODronesInCharging.Count());
                        }
                        MessageBox.Show("The details have been successfully updated");
                        refreshBaseStations();
                        initBaseStation();
                    }
                    catch (BO.UnextantException ex)
                    {
                        MessageBox.Show($"update failed. The {ex.Message} was not found in the data system");
                    }
                    catch (BO.XMLFileLoadCreateException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch(UnextantException ex) { MessageBox.Show($"refresh details failed. The {ex.Message} was not found in the data system"); }
                    catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                    update.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Initialize the details of the current base station
        /// </summary>
        private void initBaseStation()
        {
            BO.BaseStation tempBaseStation = new BO.BaseStation();
            tempBaseStation.Id = model.MyBaseStation.Id;
            try
            {
                lock (bl)
                {
                    tempBaseStation = bl.RequestBaseStation(tempBaseStation);
                }
            }
            catch(BO.UnextantException ex)
            {
                throw new UnextantException(ex.Message, ex);
            }
            catch(BO.XMLFileLoadCreateException ex)
            {
                throw new XMLFileLoadCreateException(ex.xmlFilePath, ex.Message, ex);
            }
            model.MyBaseStation.Id = tempBaseStation.Id;
            model.MyBaseStation.Name = tempBaseStation.Name;
            model.MyBaseStation.Location.Latitude = tempBaseStation.Location.Latitude;
            model.MyBaseStation.Location.Longitude = tempBaseStation.Location.Longitude;
            model.MyBaseStation.AvailableChargeSlots = tempBaseStation.AvailableChargeSlots;
            List<DroneInCharging> droneInChargings = new List<DroneInCharging>();
            droneInChargings = (from item in tempBaseStation.DronesInCharching
                                select new DroneInCharging() { BatteryStatus = item.BatteryStatus, Id = item.Id }).ToList();
            model.MyBaseStation.ODronesInCharging = droneInChargings;
        }

        /// <summary>
        /// open the DroneView window of the selected drone in charging 
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second EventArgs type</param>
        private void DroneSelectionChanged(object sender, EventArgs e)
        {
            PO.DroneInCharging d = (PO.DroneInCharging)DronesInChargingSelector.SelectedItem;
            BO.Drone drone = new BO.Drone()
            {
                Id = d.Id,
            };
            try
            {
                lock (bl)
                {
                    new DroneView(bl, bl.RequestDrone(drone)).Show();
                }
            }
            catch (BO.UnextantException ex)
            {
                if(ex.Message == "drone")
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
            catch(BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            buttonCancel = true;
            this.Close();
            buttonCancel = false;
        }

        /// <summary>
        /// delete base station if all the charge slots are available
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lock (bl)
                {
                    if (bl.DeleteBaseStation(model.MyBaseStation.Id) == true)
                    {
                        MessageBox.Show("the base station was deleted successfully", "delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                        BaseStationsListModel.Instance.RefreShBaseStations();
                    }
                    else
                    {
                        MessageBox.Show($"failed to delete the base station because there are drones which are in charging there");
                    }
                }
            }
            catch(BO.UnextantException ex) { MessageBox.Show($"The chosen {ex.Message} was not found in the data system to delete it"); }
            catch(BO.XMLFileLoadCreateException ex) {
                MessageBox.Show(ex.Message);
            }
            catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }
    }
}

