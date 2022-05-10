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
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        private BlApi.IBL bl;
        private Action refreshCustomers;
        private bool buttonCancel = false;
        CustomerModel model = new CustomerModel();

        /// <summary>
        /// Initialize the Components initialize the current customer, initialize the details and the action predicate
        /// </summary>
        /// <param name="bl">first IBL.IBL object</param>
        /// <param name="drone">second BO.DroneForList object</param>
        /// <param name="initialize">3th Action predicate</param>
        public CustomerView(BlApi.IBL bl, PO.CustomerForList c)
        {
            BO.Customer boCustomer = new BO.Customer() { Id = c.Id };
            try
            {
                lock (bl)
                {
                    boCustomer = bl.RequestCustomer(boCustomer);
                }
            }
            catch(BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"There was a problem in update details of the chosen customer, {ex.Message}");
            }
            catch (BO.UnextantException ex)
            {
                if (ex.Message == "customer")
                {
                    MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                }
                else
                {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                        $"the details of the chosen customer");
                }
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.DataContext = model;
            model.IsAdd = false;
            this.bl = bl;
            InitializeComponent();
            refreshCustomers = CustomersListModel.Instance.RefreShCustomers;
            Customer customer = new Customer()
            {
                Id = boCustomer.Id,
                Name = boCustomer.Name,
                PhoneNumber = boCustomer.PhoneNumber,
                Location = new Location() { Latitude = boCustomer.Location.Latitude, Longitude = boCustomer.Location.Longitude},
            };
            foreach (BO.ParcelToCustomer item in boCustomer.FromCustomer)
            {
                customer.ofromCustomer.Add(new ParcelToCustomer()
                {
                    Id = item.Id,
                    Weight = (PO.Enum.WeightCategories)item.Weight,
                    Priority = (PO.Enum.Priorities)item.Priority,
                    Status = (PO.Enum.DroneStatuses)item.Status,
                    Partner = new CustomerInParcel() { Id = item.Partner.Id, Name = item.Partner.Name }
                });
            }
            foreach (BO.ParcelToCustomer item in boCustomer.ToCustomer)
            {
                customer.otoCustomer.Add(new ParcelToCustomer()
                {
                    Id = item.Id,
                    Weight = (PO.Enum.WeightCategories)item.Weight,
                    Priority = (PO.Enum.Priorities)item.Priority,
                    Status = (PO.Enum.DroneStatuses)item.Status,
                    Partner = new CustomerInParcel() { Id = item.Partner.Id, Name = item.Partner.Name }
                });
            }
            model.MyCustomer = customer;
            model = model.GetCustomerModel(boCustomer);
            Refresh.RefreshCustomer = initCustomer;
        }

        /// <summary>
        /// initialize components and values of text boxes
        /// so that the user will be able to fill the details of the customer
        /// </summary>
        /// <param name="bl">first IBL object</param>
        /// <param name="initilalizeDrones">second Action delegate</param>
        public CustomerView(BlApi.IBL bl)
        {
            this.DataContext = model;
            model.IsAdd = true;
            this.bl = bl;
            refreshCustomers = CustomersListModel.Instance.RefreShCustomers;
            InitializeComponent();
            init(textID, "customer id");
            init(textName, "customer name");
            init(textPhoneNumber, "phoneNumber");
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
                    if (sender == textName)
                    {
                        ((TextBox)sender).Text = "customer name";
                    }
                    if (sender == textID)
                    {
                        ((TextBox)sender).Text = "customer id";
                    }
                    if (sender == textPhoneNumber)
                    {
                        ((TextBox)sender).Text = "phoneNumber";
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
        /// add customer to the customer collection
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void Add(object sender, RoutedEventArgs e)
        {
            if (checkValid() == true)
            {
                BO.Customer c = new BO.Customer();
                c.Id = int.Parse(textID.Text);
                c.Name = textName.Text;
                c.PhoneNumber = textPhoneNumber.Text;
                c.Location.Longitude = double.Parse(textLongitude.Text);
                c.Location.Latitude = double.Parse(textLatitude.Text);
                try
                {
                    lock (bl)
                    {
                        bl.CreateCustomer(c);
                    }
                    if (MessageBox.Show("the customer was seccessfully added", "success", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                    }
                    refreshCustomers();
                }
                catch (BO.ExtantException ex)
                {
                    MessageBox.Show($"the {ex.Message} is almost exist in the data system");
                }
                catch (BO.XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch(XMLFileLoadCreateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// check if the details which the customer entered are in valid format
        /// </summary>
        /// <returns>bool</returns>
        private bool checkValid()
        {
            if (textID.Text == "" || textID.Text == "customer id" || textName.Text == "" || textName.Text == "customer name"
            || textLongitude.Text == "" || textLatitude.Text == "" || textPhoneNumber.Text == "" || textPhoneNumber.Text == "phoneNumber")
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("All fields are required", "warning", button, icon);
                return false;
            }
            else
            {
                if (textID.Background == Brushes.DarkRed || textID.Text.Length != 9 
                    || textID.Text=="000000000")
                {
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show("not valid id", "warning", button, icon);
                    return false;
                }
                else
                {
                    if (textPhoneNumber.Background == Brushes.DarkRed || textPhoneNumber.Text.Length < 7 || textPhoneNumber.Text.Length > 20)
                    {
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBox.Show("not valid phoneNumber", "warning", button, icon);
                        return false;
                    }
                    if (textLongitude.Background == Brushes.DarkRed || textLatitude.Background == Brushes.DarkRed ||
                        double.Parse(textLatitude.Text) < 0 || double.Parse(textLatitude.Text) > 180 ||
                        double.Parse(textLongitude.Text) < 0 || double.Parse(textLongitude.Text) > 180)
                    {
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBox.Show("not valid location", "warning", button, icon);
                        return false;
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
        /// Initialize the details of the current customer
        /// </summary>
        private void initCustomer()
        {
            BO.Customer tempCustomer = new BO.Customer();
            tempCustomer.Id = model.MyCustomer.Id;
            try
            {
                lock (bl)
                {
                    tempCustomer = bl.RequestCustomer(tempCustomer);
                }
            }
            catch (BO.DiscrepanciesException ex) {
                throw new DiscrepanciesException(ex.Message, ex);
            }
            catch(BO.UnextantException ex)
            {
                throw new UnextantException(ex.Message, ex);
            }
            catch(BO.XMLFileLoadCreateException ex)
            {
                throw new XMLFileLoadCreateException(ex.xmlFilePath, ex.Message, ex);
            }
            PO.Customer customer = new Customer();
            customer.Id = tempCustomer.Id;
            customer.Name = tempCustomer.Name;
            customer.PhoneNumber = tempCustomer.PhoneNumber;
            IEnumerable<BO.ParcelForList> fromCustomer = Enumerable.Empty<BO.ParcelForList>();
            IEnumerable<BO.ParcelForList> toCustomer = Enumerable.Empty<BO.ParcelForList>();
            try
            {
                lock (bl)
                {
                    fromCustomer = bl.RequestPartListParcels(parcel => parcel.SenderName == tempCustomer.Name);
                    toCustomer = bl.RequestPartListParcels(parcel => parcel.TargetName == tempCustomer.Name);
                }
            }
            catch(BO.XMLFileLoadCreateException ex)
            {
                throw new XMLFileLoadCreateException(ex.xmlFilePath, ex.Message, ex);
            }

            foreach (var item in fromCustomer)
            {
                customer.ofromCustomer.Add(new ParcelToCustomer() { Id = item.Id });
            }
            foreach (var item in toCustomer)
            {
                customer.otoCustomer.Add(new ParcelToCustomer() { Id = item.Id });
            }
            model.MyCustomer = customer;
        }
       
        /// <summary>
        /// Enabled to update
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second KeyboardFocusChangedEventArgs type</param>
        private void UpdateCustomer(object sender, KeyboardFocusChangedEventArgs e)
        {
            update.IsEnabled = true;
        }

        /// <summary>
        /// the Click event of the Button control is connected to the event handler method
        /// in addition this method update the customer
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ButtonUpdate(object sender, RoutedEventArgs e)
        {
            if (PhoneNumber.Background == Brushes.DarkRed || (PhoneNumber.Text!="" && (PhoneNumber.Text.Length < 7 || PhoneNumber.Text.Length > 20)))
            {
                MessageBox.Show("not valid input");
            }
            else
            {
                if (Name.Text == "" && PhoneNumber.Text == "")
                {
                    MessageBox.Show("no details to update");
                }
                else
                {
                    BO.Customer c = new BO.Customer();
                    c.Id = model.MyCustomer.Id;
                    if (Name.Text != "")
                        c.Name = Name.Text;
                    if (PhoneNumber.Background == Brushes.LightGray && PhoneNumber.Text != "")
                        c.PhoneNumber = PhoneNumber.Text;
                    try
                    {
                        lock (bl)
                        {
                            bl.UpdateCustomer(c);
                        }
                        MessageBox.Show("The details have been successfully updated");
                        initCustomer();
                        refreshCustomers();
                    }
                    catch (BO.UnextantException ex)
                    {
                        MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                    }
                    catch (BO.XMLFileLoadCreateException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch(DiscrepanciesException ex) { MessageBox.Show($"there was a problem in refreshing details, {ex.Message}"); }
                    catch(UnextantException ex) 
                    {
                        if (ex.Message == "customer")
                        {
                            MessageBox.Show($"The chosen {ex.Message} was not found in the data system");
                        }
                        else
                        {
                            MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to update " +
                                $"the details of the chosen customer");
                        }
                    }
                    catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
                    update.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// open the ParcelView window of the selected parcel
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second SelectionChangedEventArgs type</param>
        private void ParcelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Parcel p = new BO.Parcel();
            p.Id = ((ParcelToCustomer)((ComboBox)sender).SelectedItem).Id;
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
                        $"the details of the chosen parcel");
                }
            }
            catch (BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"there was a problem in update the details of the chosen parcel, {ex.Message}");
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// delete customer
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lock (bl)
                {
                    if (bl.DeleteCustomer(model.MyCustomer.Id) == true)
                    {
                        MessageBox.Show("the customer was deleted successfully", "delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        buttonCancel = true;
                        this.Close();
                        buttonCancel = false;
                        CustomersListModel.Instance.RefreShCustomers();
                    }
                    else
                    {
                        MessageBox.Show("failed to delete the customer because parcels which are connected to him are almost in treating");
                    }
                }
            }
            catch(BO.UnextantException ex) { MessageBox.Show($"the {ex.Message} was not found in the data system to delete"); }
            catch(BO.XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }
    }
}

