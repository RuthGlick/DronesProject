using PL.Model;
using PO;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneView.xaml
    /// </summary>
    public partial class DroneView : Window, INotifyPropertyChanged
    {
        private BlApi.IBL bl;
        private Action refreshDrones;
        private bool buttonCancel = false;
        DroneModel model = new DroneModel();
        bool auto;
        bool close = false;

        public bool Auto
        {
            get => auto;
            set => this.setAndNotify(PropertyChanged, nameof(Auto), out auto, value);
        }

        /// <summary>
        /// Initialize the Components initialize the current drone, initialize the details and the action predicate
        /// </summary>
        /// <param name="bl">first IBL.IBL object</param>
        /// <param name="drone">second BO.DroneForList object</param>
        public DroneView(BlApi.IBL bl, BO.Drone drone)
        {
            model.IsAdd = false;
            InitializeComponent();
            this.bl = bl;
            Drone newDrone;
            newDrone = new Drone()
            {
                Id = drone.Id,
                Battery = drone.Battery,
                Location = new Location() { Longitude = drone.Location.Longitude, Latitude = drone.Location.Latitude },
                Model = drone.Model,
                Status = drone.Status,
                Weight = drone.Weight,
            };
            if (drone.DeliveryByTransfer != null)
            {
                newDrone.DeliveryByTransfer = new ParcelInTransfer();
                initDeliveryByTransfer(newDrone.DeliveryByTransfer, drone.DeliveryByTransfer);
                if (ThirdButton.IsEnabled == true)
                {
                    ForthButton.IsEnabled = false;
                }
                else
                {
                    ForthButton.IsEnabled = true;
                }
            }
            updateModel.IsEnabled = false;
            refreshDrones = DronesListModel.Instance.RefreshDrones;
            model = model.GetDroneModel(newDrone);
            this.DataContext = model;
            Refresh.RefreshDrone = initDrone;
        }

        /// <summary>
        /// initialize components and values of radio buttons, text boxes and combo box
        /// so that the user will be able to fill the details of the drone
        /// </summary>
        /// <param name="bl">first IBL object</param>
        public DroneView(BlApi.IBL bl)
        {
            model.IsAdd = true;
            this.DataContext = model;
            this.bl = bl;
            refreshDrones = DronesListModel.Instance.RefreshDrones;
            InitializeComponent();
            init(textID, "drone id (5 characters)");
            init(textModel, "drone model");
            RB1.Content = (BL.Enum.WeightCategories.HEAVY);
            RB2.Content = (BL.Enum.WeightCategories.INTERMEDIATE);
            RB3.Content = (BL.Enum.WeightCategories.LIGHT);
            try
            {
                lock (bl)
                {
                    numStation.ItemsSource = bl.RequestListAvailableChargeSlots().Select(baseStation => baseStation.Id);
                }
            }
            catch (BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
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
            textBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(textID_GotKeyboardFocus);
            textBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(textID_LostKeyboardFocus);
        }

        /// <summary>
        /// erase the text of a textbox chief when the focus on it 
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second KeyboardFocusChangedEventArgs type</param>
        private void textID_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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
        /// the text which tell the user what to fill in gray.
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second KeyboardFocusChangedEventArgs type</param>
        private void textID_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Make sure sender is the correct Control.
            if (sender is TextBox)
            {
                //If nothing was entered, reset default text.
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    if (sender == textModel)
                    {
                        ((TextBox)sender).Text = "drone model";
                    }
                    if (sender == textID)
                    {
                        ((TextBox)sender).Text = "drone id (5 characters)";
                    }
                }
            }
        }

        /// <summary>
        /// add drone to the drone collection
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Add(object sender, RoutedEventArgs e)
        {
            if (textID.Text == "" || textID.Text == "drone id (5 characters)" || textModel.Text == "" || textModel.Text == "drone model"
                || (RB1.IsChecked == false && RB2.IsChecked == false && RB3.IsChecked == false) || numStation.SelectedItem == null)
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("All fields are required", "warning", button, icon);
            }
            else
            {
                if (textID.Background == Brushes.DarkRed || textID.Text.Length != 5 || textID.Text == "00000")
                {
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show("not valid input", "warning", button, icon);
                }
                else
                {
                    BO.Drone drone = new BO.Drone();
                    drone.Id = int.Parse(textID.Text);
                    drone.Model = textModel.Text;
                    if (RB1.IsChecked == true) drone.Weight = (BL.Enum.WeightCategories)RB1.Content;
                    if (RB2.IsChecked == true) drone.Weight = (BL.Enum.WeightCategories)RB2.Content;
                    if (RB3.IsChecked == true) drone.Weight = (BL.Enum.WeightCategories)RB3.Content;

                    try
                    {
                        lock (bl)
                        {
                            bl.CreateDrone(drone, (int)numStation.SelectedItem);
                        }
                        if (MessageBox.Show("the drone was seccessfully added", "success", MessageBoxButton.OK) == MessageBoxResult.OK)
                        {
                            realyClose();
                        }
                        refreshDrones();
                        BaseStationsListModel.Instance.RefreShBaseStations();
                    }
                    catch (BO.UnextantException ex)
                    {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to create " +
                            $"the new drone");
                    }
                    catch (BO.ExtantException ex)
                    {
                        MessageBox.Show($"the {ex.Message} is almost exist in the data system");
                    }
                    catch (BO.XMLFileLoadCreateException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (UnextantException ex)
                    {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh details " +
                            $"after adding a drone");
                    }
                    catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                }
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
        /// Initialize Delivery By Transfer
        /// </summary>
        /// <param name="po">first PO.ParcelInTransfer object</param>
        /// <param name="bo">second BO.ParcelInTransfer object</param>
        private void initDeliveryByTransfer(PO.ParcelInTransfer po, BO.ParcelInTransfer bo)
        {
            po.Id = bo.Id;
            po.Location = new Location() { Latitude = bo.Location.Latitude, Longitude = bo.Location.Longitude };
            po.Priority = (PO.Enum.Priorities)bo.Priority;
            po.Sender = new CustomerInParcel() { Id = bo.Sender.Id, Name = bo.Sender.Name };
            po.Target = new CustomerInParcel() { Id = bo.Target.Id, Name = bo.Target.Name };
            po.Destination = new Location() { Longitude = bo.Destination.Longitude, Latitude = bo.Destination.Latitude };
            po.Status = bo.status;
            po.Weight = (PO.Enum.WeightCategories)bo.Weight;
            po.TransportDistance = bo.TransportDistance;
        }

        /// <summary>
        /// Initialize the details of the current drone
        /// </summary>
        private void initDrone()
        {
            if (model.MyDrone == null) return;
            BO.Drone temp = new BO.Drone();
            temp.Id = model.MyDrone.Id;
            try
            {
                lock (bl)
                {
                    temp = bl.RequestDrone(temp);
                }
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                throw new XMLFileLoadCreateException(ex.xmlFilePath, ex.Message, ex);
            }
            catch (BO.DiscrepanciesException ex)
            {
                throw new DiscrepanciesException(ex.Message, ex);
            }
            catch (BO.UnextantException ex)
            {
                throw new UnextantException(ex.Message, ex);
            }
            Drone drone = new Drone();
            drone.Id = temp.Id;
            drone.Model = temp.Model;
            drone.Battery = temp.Battery;
            drone.Location = new Location();
            drone.Location.Longitude = temp.Location.Longitude;
            drone.Location.Latitude = temp.Location.Latitude;
            drone.Status = temp.Status;
            drone.Weight = temp.Weight;
            drone.DeliveryByTransfer = new ParcelInTransfer();
            if (temp.DeliveryByTransfer != null)
            {
                if (temp.DeliveryByTransfer.Id == 0)
                {
                    drone.DeliveryByTransfer = null;
                }
                else
                {
                    initDeliveryByTransfer(drone.DeliveryByTransfer, temp.DeliveryByTransfer);
                }
            }
            else
            {
                drone.DeliveryByTransfer = null;
            }
            model.MyDrone = drone;
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method close the window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            if (Auto == true)
            {
                Manual_Click();
                close = true;
            }
            else
            {
                realyClose();
            }
        }

        /// <summary>
        /// The function allows the display of "updateModel" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateModel(object sender, KeyboardFocusChangedEventArgs e)
        {
            updateModel.IsEnabled = true;
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method update the model of drone
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            BO.Drone drone = new BO.Drone();
            drone.Id = model.MyDrone.Id;
            drone.Model = Model.Text;
            try
            {
                lock (bl)
                {
                    bl.UpdateDrone(drone);
                }
                MessageBox.Show("The drone has been successfully updated");
                initDrone();
                refreshDrones();
            }
            catch (BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            catch (BO.UnextantException ex)
            {
                MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
            }
            catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            catch (DiscrepanciesException ex) { MessageBox.Show($"failed to update the details of the chosen drone, {ex.Message}"); }
            catch (UnextantException ex)
            {
                MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                    $"the details of the drones");
            }
            updateModel.IsEnabled = false;
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this the function performs the operation for the request written in the contents of the third button
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void button_3(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            BO.Drone d = new BO.Drone();
            d.Id = model.MyDrone.Id;
            switch (content)
            {
                case "charge drone":
                    {
                        try
                        {
                            lock (bl)
                            {
                                bl.UpdateCharge(d);
                            }
                            MessageBox.Show("The drone was sent for loading");
                            initDrone();
                            refreshDrones();
                            BaseStationsListModel.Instance.RefreShBaseStations();
                            Refresh.RefreshBaseStation();
                        }
                        catch (BO.UnextantException ex)
                        {
                            if (ex.Message == "drone charge")
                            {
                                MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                            }
                            else
                            {
                                MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                                    $"charging");
                            }
                        }
                        catch (BO.DiscrepanciesException ex)
                        {
                            MessageBox.Show($"failed to update the charging of the drone, {ex.Message}");
                        }
                        catch (BO.XMLFileLoadCreateException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        catch (BO.ExtantException ex)
                        {
                            MessageBox.Show($"the {ex.Message} is almost exist in the data system, you can not charge the drone before release it");
                        }
                        catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                        catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the drones, {ex.Message}"); }
                        catch (UnextantException ex)
                        {
                            MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                                $"the details after update charging");
                        }
                        break;
                    }
                case "release from charging":
                    {
                        time.Visibility = Visibility.Visible;
                        ThirdButton.IsEnabled = false;
                        break;
                    }
                case "pick up a parcel":
                    {
                        MessageBoxResult result = MessageBox.Show("Are you sure you want to pick up the parcel?",
                            "Pick Up", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                lock (bl)
                                {
                                    bl.UpdatePickedUp(d);
                                }
                                MessageBox.Show("The package was collected by a drone");
                                initDrone();
                                refreshDrones();
                                CustomersListModel.Instance.RefreShCustomers();
                                ParcelsListModel.Instance.RefreShParcels();
                                Refresh.RefreshParcel();
                                ForthButton.IsEnabled = true;
                                ThirdButton.IsEnabled = false;
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
                                        $"pick up a parcel");
                                }
                            }
                            catch (BO.DiscrepanciesException ex)
                            {
                                MessageBox.Show($"failed to update that the drone picked up a parcel, {ex.Message}");
                            }
                            catch (UnextantException ex)
                            {
                                MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                                    $"the details after update pich up a parcel");
                            }
                            catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details after picking up a parcel, {ex.Message}"); }
                            catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this the function performs the operation for the request written in the contents of the 4th button
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void button_4(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            BO.Drone d = new BO.Drone();
            d.Id = model.MyDrone.Id;
            switch (content)
            {
                case "send to transport":
                    {
                        try
                        {
                            lock (bl)
                            {
                                bl.UpdateScheduled(d);
                            }
                            MessageBox.Show("The package was associated with a drone");
                            initDrone();
                            refreshDrones();
                            ParcelsListModel.Instance.RefreShParcels();
                            Refresh.RefreshParcel();
                            ForthButton.IsEnabled = false;
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
                                    $"schedule a parcel to drone");
                            }
                        }
                        catch (BO.DiscrepanciesException ex)
                        {
                            MessageBox.Show($"failed to update that a parcel was scheduled to a drone, {ex.Message}");
                        }
                        catch (BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                        catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                        catch (UnextantException ex)
                        {
                            MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                                $"the details after update schedule");
                        }
                        catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of schedule a parcel to a drone, {ex.Message}"); }
                        break;
                    }
                case "supply":
                    {
                        MessageBoxResult result = MessageBox.Show("Are you sure you want to supply the parcel?",
                             "Supply", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                lock (bl)
                                {
                                    bl.UpdateSupply(d);
                                }
                                initDrone();
                                refreshDrones();
                                ParcelsListModel.Instance.RefreShParcels();
                                Refresh.RefreshParcel();
                                Refresh.RefreshCustomer();
                                CustomersListModel.Instance.RefreShCustomers();
                                ThirdButton.IsEnabled = true;
                                ForthButton.IsEnabled = true;
                                MessageBox.Show("The package was provided by the drone");
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
                                        $"supply a parcel");
                                }
                            }
                            catch (BO.DiscrepanciesException ex)
                            {
                                MessageBox.Show($"failed to update that a drone provided the parcel, {ex.Message}");
                            }
                            catch (BO.XMLFileLoadCreateException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                            catch (UnextantException ex)
                            {
                                MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh details " +
                                    $"after supply a parcel");
                            }
                            catch (DiscrepanciesException ex) { MessageBox.Show($"failed to update the details after the parcel was provided, {ex.Message}"); }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// open the ParcelView window of the selected parcel
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second SelectionChangedEventArgs type</param>
        private void Select(object sender, EventArgs e)
        {
            if (model.MyDrone.DeliveryByTransfer != null && model.MyDrone.DeliveryByTransfer.Id != 0)
            {
                BO.Parcel p = new BO.Parcel();
                p.Id = int.Parse(Parcel.Text);
                try
                {
                    lock (bl)
                    {
                        new ParcelView(bl, bl.RequestParcel(p)).Show();
                    }
                }
                catch (BO.UnextantException ex)
                {
                    if (ex.Message == "parcel")
                    {
                        MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                    }
                    else
                    {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                            $"details of the chosen parcel");
                    }
                }
                catch (BO.DiscrepanciesException ex)
                {
                    MessageBox.Show($"failed to update the details of the chosen parcel, {ex.Message}");
                }
                catch (BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method release the drone frome charging
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void releaseButton(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("Required field", "warning", button, icon);
            }
            else
            {
                BO.DroneInCharging droneInCharging = new BO.DroneInCharging();
                droneInCharging.Id = model.MyDrone.Id;
                int timeOfCharging = int.Parse(textBox.Text);
                try
                {
                    lock (bl)
                    {
                        bl.UpdateRelease(droneInCharging, 60 * timeOfCharging);
                    }
                    if (MessageBox.Show("The drone was released from loading", "released", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        initDrone();
                        refreshDrones();
                        BaseStationsListModel.Instance.RefreShBaseStations();
                        Refresh.RefreshBaseStation();
                        ThirdButton.IsEnabled = true;
                    }
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
                            $"release a drone from charging");
                    }
                }
                catch (BO.DiscrepanciesException ex)
                {
                    MessageBox.Show($"failed to update that a drone was released from charging, {ex.Message}");
                }
                catch (BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (UnextantException ex)
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh details " +
                        $"after release a drone from charging");
                }
                catch (DiscrepanciesException ex) { MessageBox.Show($"failed to update the details after the drone was released from charging, {ex.Message}"); }
                catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                textBox.Text = "";
                time.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// delete drone
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lock (bl)
                {
                    if (bl.DeleteDrone(model.MyDrone.Id) == true)
                    {
                        MessageBox.Show("the drone was deleted successfully", "delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        realyClose();
                        DronesListModel.Instance.RefreshDrones();
                        BaseStationsListModel.Instance.RefreShBaseStations();
                        Refresh.RefreshBaseStation();
                    }
                    else
                    {
                        MessageBox.Show("failed to delete the drone because it is in active");
                    }
                }
            }
            catch (BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            catch (BO.UnextantException ex)
            {
                if (ex.Message == "drone")
                {
                    MessageBox.Show($"The chosen {ex.Message} was not found in the data system to delete");
                }
                else
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to delete " +
                        $"the drone");
                }
            }
            catch (UnextantException ex)
            {
                MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh details " +
                    $"after delete a drone");
            }
            catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// close the window
        /// </summary>
        private void realyClose()
        {
            buttonCancel = true;
            this.Close();
            buttonCancel = false;
        }

        /// <summary>
        /// Requires the input to be only digits
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void PreviewText_Input(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        BackgroundWorker worker;

        /// <summary>
        /// update drone, call to report progrss
        /// </summary>
        private void updateDrone() => worker.ReportProgress(0);

        /// <summary>
        /// check if to stop the simulator
        /// </summary>
        /// <returns></returns>
        private bool checkStop() => worker.CancellationPending;

        /// <summary>
        /// sets off the simulator
        /// </summary>
        /// <param name="sender">the first object value</param>
        /// <param name="e">the second RoutedEventArgs value</param>
        private void SimulatorB(object sender, RoutedEventArgs e)
        {
            if ((string)SimulatorButton.Content == "Simulator")
            {
                SimulatorButton.Content = "Manual";
                buttons.Visibility = Visibility.Collapsed;
                Auto_Click(sender, e);
            }
            else
            {
                Manual_Click();
            }
        }

        /// <summary>
        /// Initializes and activates the bacjgroundWorker
        /// </summary>
        /// <param name="sender">the first object value</param>
        /// <param name="e">the second RoutedEventArgs value</param>
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            Auto = true;
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
            lock (bl)
            {
                worker.DoWork += (sender, args) =>
                {
                    try
                    {
                        bl.StartDroneSimulator((int)args.Argument, updateDrone, checkStop);
                    }
                    catch (BO.UnextantException ex)
                    {
                        MessageBox.Show($"{ex.Message} is not exist in the data system");
                    }
                    catch (BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                    catch (BO.DiscrepanciesException ex) { MessageBox.Show($"there are discrepancies between data and request, {ex.Message}"); }
                    catch (BO.ExtantException ex) { MessageBox.Show($"{ex.Message} is almost extant in the data system"); }

                };
            }
            worker.RunWorkerCompleted += (sender, args) => complete();
            worker.ProgressChanged += (sender, args) => process();
            worker.RunWorkerAsync(model.MyDrone.Id);
        }

        /// <summary>
        /// change the display to Manual mode display
        /// </summary>
        private void complete()
        {
            Auto = false;
            SimulatorButton.Content = "Simulator";
            closeButton.Visibility = updateModel.Visibility = delete.Visibility = Visibility.Visible;
            buttons.Visibility = Visibility.Visible;
            if (close == true)
            {
                realyClose();
                close = false;
            }
        }

        /// <summary>
        /// refresh the windows - progressChange
        /// </summary>
        private void process()
        {
            if (model.MyDrone != null)
            {
                try
                {
                    initDrone();
                    refreshDrones();
                    ParcelsListModel.Instance.RefreShParcels();
                    if (model.MyDrone.DeliveryByTransfer != null)
                    {
                        if (Refresh.RefreshParcel != null)
                            Refresh.RefreshParcel();
                        if (Refresh.RefreshCustomer != null)
                            Refresh.RefreshCustomer();
                    }
                    ParcelsListModel.Instance.RefreShParcels();
                    if (Refresh.RefreshBaseStation != null)
                        Refresh.RefreshBaseStation();
                    BaseStationsListModel.Instance.RefreShBaseStations();
                }
                catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the open windows, {ex.Message}"); }
                catch (UnextantException ex)
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                        $"details in the simulator");
                }
            }
        }

        /// <summary>
        /// Soft closure
        /// </summary>
        private void Manual_Click()
        {
            worker?.CancelAsync();
        }
    }
}

