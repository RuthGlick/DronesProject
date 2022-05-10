using PL.Model;
using PO;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelView.xaml
    /// </summary>
    public partial class ParcelView : Window
    {
        private BlApi.IBL bl;
        private Action refreshParcels;
        private bool buttonCancel = false;
        ParcelModel model=new ParcelModel();

        /// <summary>
        /// Initialize the Components initialize the current parcel, initialize the details and the action predicate
        /// </summary>
        /// <param name="bl">first IBL.IBL object</param>
        /// <param name="drone">second BO.Parcel object</param>
        public ParcelView(BlApi.IBL bl, BO.Parcel p)
        {
            model.IsAdd = false;
            this.bl = bl;
            InitializeComponent();
            refreshParcels = ParcelsListModel.Instance.RefreShParcels;
            Parcel parcel = new Parcel();
            parcel.Id = p.Id;
            parcel.Sender = new CustomerInParcel() { Id = p.Sender.Id, Name = p.Sender.Name };
            parcel.Target = new CustomerInParcel() { Id = p.Target.Id, Name = p.Target.Name };
            parcel.Weight = (PO.Enum.WeightCategories)p.Weight;
            parcel.Priority = (PO.Enum.Priorities)p.Priority;
            parcel.DroneInParcel = new DroneInParcel();
            if(p.Drone1 != null)
            {
                parcel.DroneInParcel.Id = p.Drone1.Id;
                parcel.DroneInParcel.BatteryStatus = p.Drone1.BatteryStatus;
                parcel.DroneInParcel.Current = new Location();
                parcel.DroneInParcel.Current.Longitude = p.Drone1.Current.Longitude;
                parcel.DroneInParcel.Current.Latitude = p.Drone1.Current.Latitude;
            }
            parcel.Created = p.Created;
            parcel.Scheduled = p.Scheduled;
            parcel.PickedUp = p.PickedUp;
            parcel.Delivered = p.Delivered;
            model = model.GetParcelModel(parcel);
            this.DataContext = model;
            Refresh.RefreshParcel = InitParcel;
        }

        /// <summary>
        /// initialize components and values of  text boxes and combo box
        /// so that the user will be able to fill the details of the parcel
        /// </summary>
        /// <param name="bl">first IBL object</param>   
        public ParcelView(BlApi.IBL bl)
        {
            model.IsAdd = true;
            this.DataContext = model;
            this.bl = bl;
            refreshParcels = ParcelsListModel.Instance.RefreShParcels;
            InitializeComponent();
            init(senderId, "sender id");
            init(targetId, "target id");
            AddWeight.ItemsSource = System.Enum.GetValues(typeof(PO.Enum.WeightCategories));
            AddPriority.ItemsSource = System.Enum.GetValues(typeof(PO.Enum.Priorities));
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
        /// the text which tell the user what to fill in gray.
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
                    if (((TextBox)sender).Text.Trim().Equals(""))
                    {
                        ((TextBox)sender).Foreground = Brushes.Gray;
                        if (sender == senderId)
                        {
                            ((TextBox)sender).Text = "sender id";
                        }
                        if (sender == targetId)
                        {
                            ((TextBox)sender).Text = "target id";
                        }
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
        /// add parcel to the parcel collection
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Add(object sender, RoutedEventArgs e)
        {
            if (checkValid() == true)
            {
                BO.Parcel p = new BO.Parcel();
                p.Sender = new BO.CustomerInParcel() { Id = int.Parse(senderId.Text) };
                p.Target = new BO.CustomerInParcel() { Id = int.Parse(targetId.Text) };
                p.Weight = (BL.Enum.WeightCategories)AddWeight.SelectedItem;
                p.Priority = (BL.Enum.Priorities)AddPriority.SelectedItem;
                p.Drone1 = null;
                try
                {
                    lock (bl)
                    {
                        bl.CreateParcel(p);
                    }
                    if (MessageBox.Show("the parcel was seccessfully added", "success", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                    }
                    refreshParcels();
                }
                catch (BO.UnextantException ex)
                {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                            $"the adding of the parcel");
                }
                catch (BO.ExtantException ex)
                {
                    MessageBox.Show($"the {ex.Message} is almost exist in the data system");
                }
                catch (BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch(DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the parcels, {ex.Message}"); }
                catch(UnextantException ex) {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                            $"the details after adding a parcel");
                }
                catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            }
        }

        /// <summary>
        /// check if the details which the parcel entered are in valid format
        /// </summary>
        /// <returns>bool</returns>
        private bool checkValid()
        {
            if (senderId.Text == "" || senderId.Text == "sender id" || targetId.Text == "" || targetId.Text == "target id"
            || AddWeight.SelectedItem == null || AddPriority.SelectedItem == null)
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("All fields are required", "warning", button, icon);
                return false;
            }
            else
            {
                if (senderId.Text.Length != 9 || targetId.Text.Length != 9 ||
                    senderId.Background == Brushes.DarkRed || targetId.Background == Brushes.DarkRed
                    || senderId.Text == "000000000" || targetId.Text == "000000000")
                {
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show("not valid id", "warning", button, icon);
                    return false;
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
        /// open CustomerView window accordind to the selected item
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            if (e.Source == CustomerTarget)
            {
                CustomerForList c = new CustomerForList() { Id = model.MyParcel.Target.Id };
                new CustomerView(bl, c).Show();
            }
            if (e.Source == CustomerSender)
            {
                CustomerForList c = new CustomerForList() { Id = model.MyParcel.Sender.Id };
                new CustomerView(bl, c).Show();
            }
        }

        /// <summary>
        /// open DroneView window accordind to the selected item
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void Button_Click_Drone(object sender, RoutedEventArgs e)
        {
            if(model.MyParcel.Scheduled != null && model.MyParcel.Delivered == null)
            {
                BO.Drone drone = new BO.Drone() { Id = model.MyParcel.DroneInParcel.Id };
                try
                {
                    lock (bl)
                    {
                        drone = bl.RequestDrone(drone);
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
                            $"the details of the chosen drone");
                    }
                }
                catch (BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.DiscrepanciesException ex)
                {
                    MessageBox.Show($"failed to update the details of the chosen drone, {ex.Message}");
                }
                new DroneView(bl, drone).Show();
            }
        }

        /// <summary>
        /// delete parcel
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lock (bl)
                {
                    if (bl.DeleteParcel(model.MyParcel.Id) == true)
                    {
                        MessageBox.Show("the parcel was deleted successfully", "delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                        ParcelsListModel.Instance.RefreShParcels();
                    }
                    else
                    {
                        MessageBox.Show("faild to delete the parcel because the parcel is in the middle of a shipping process");
                    }
                }
            }
            catch (BO.UnextantException ex) { }
            catch(BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            catch(DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the parcels, {ex.Message}"); }
            catch(UnextantException ex) {
                if (ex.Message == "parcel")
                {
                    MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                }
                else
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                        $"the details after delete a parcel");
                }
            }
            catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// init model.MyParcel
        /// </summary>
        public void InitParcel()
        {
            if (model.MyParcel == null) { return; }
            try
            {
                lock (bl)
                {
                    model.MyParcel = convertToPO(bl.RequestParcel(convertToBO(model.MyParcel)));
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
            catch(BO.DiscrepanciesException ex)
            {
                throw new DiscrepanciesException(ex.Message, ex);
            }
        }

        /// <summary>
        /// convert from Parcel to BO.Parcel
        /// </summary>
        /// <param name="parcel">the first Parcel value</param>
        /// <returns>BO.Parcel object</returns>
        private BO.Parcel convertToBO(Parcel parcel)
        {
            if (parcel == null) { return new BO.Parcel(); }
            BO.Parcel p = new BO.Parcel()
            {
                Id = parcel.Id,
                Weight = (BL.Enum.WeightCategories)parcel.Weight,
                Priority = (BL.Enum.Priorities)parcel.Priority,
                Created = parcel.Created,
                PickedUp = parcel.PickedUp,
                Scheduled = parcel.Scheduled,
                Delivered = parcel.Delivered
            };
            if (parcel.Sender != null) { p.Sender = new BO.CustomerInParcel() { Id = parcel.Sender.Id, Name = parcel.Sender.Name }; }
            if (parcel.Target != null) { p.Target = new BO.CustomerInParcel() { Id = parcel.Target.Id, Name = parcel.Target.Name }; }
            if (parcel.DroneInParcel != null)
            {
                p.Drone1 = new BO.DroneInParcel() { Id = parcel.DroneInParcel.Id, BatteryStatus = parcel.DroneInParcel.BatteryStatus };
                if (parcel.DroneInParcel.Current != null)
                {
                    p.Drone1.Current = new BO.Location() { Latitude = parcel.DroneInParcel.Current.Latitude, Longitude = parcel.DroneInParcel.Current.Longitude };
                }
            }

            return p;
        }

        /// <summary>
        /// convert from BO.Parcel to PO.Parcel
        /// </summary>
        /// <param name="parcel">the first BO.Parcel value</param>
        /// <returns>Parcel object</returns>
        private Parcel convertToPO(BO.Parcel parcel)
        {
            if (parcel == null) { return new Parcel(); }
            Parcel p = new Parcel()
            {
                Id = parcel.Id,
                Weight = (PO.Enum.WeightCategories)parcel.Weight,
                Priority = (PO.Enum.Priorities)parcel.Priority,
                Created = parcel.Created,
                PickedUp = parcel.PickedUp,
                Scheduled = parcel.Scheduled,
                Delivered = parcel.Delivered
            };
            if (parcel.Sender != null) { p.Sender = new CustomerInParcel() { Id = parcel.Sender.Id, Name = parcel.Sender.Name }; }
            if (parcel.Target != null) { p.Target = new CustomerInParcel() { Id = parcel.Target.Id, Name = parcel.Target.Name }; }
            if (parcel.Drone1 != null)
            {
                p.DroneInParcel = new DroneInParcel() { Id = parcel.Drone1.Id, BatteryStatus = parcel.Drone1.BatteryStatus };
                if (parcel.Drone1.Current != null)
                {
                    p.DroneInParcel.Current = new Location() { Latitude = parcel.Drone1.Current.Latitude, Longitude = parcel.Drone1.Current.Longitude };
                }
            }

            return p;
        }
    }
}