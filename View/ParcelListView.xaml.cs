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
    /// Interaction logic for ParcelListView.xaml
    /// </summary>
    public partial class ParcelListView : Window
    {
        private BlApi.IBL bl;
        private bool buttonCancel = false;
        private PL.Model.ParcelsListModel parcelsModel;
        private int statusChosen = -1;
        private int priorityChosen = -1;
        private int weightChosen = -1;
        private SelectedDatesCollection dateChosen = default(SelectedDatesCollection);

        /// <summary>
        /// initialize components, the list of drones and 3 combo box of options which parcelr to show
        /// </summary>
        /// <param name="bl">first IBL type</param>
        public ParcelListView(BlApi.IBL bl)
        {
            this.bl = bl;
            InitializeComponent();
            parcelsModel = PL.Model.ParcelsListModel.Instance;
            parcelsModel.ParcelView.Filter = FilterParcel;
            DataContext = parcelsModel;
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(BL.Enum.Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BL.Enum.DeliveryStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BL.Enum.WeightCategories));
        }

        /// <summary>
        /// filter parcel acording to Status and Weight,Priority and dateChosen
        /// </summary>
        /// <param name="obj">first object type</param>
        /// <returns>bool</returns>
        private bool FilterParcel(object obj)
        {
            if (obj is PO.ParcelForList parcel)
            {
                return (statusChosen == -1 || (int)parcel.Status == statusChosen)
                    && (weightChosen == -1 || (int)parcel.Weight == weightChosen)
                    && (priorityChosen == -1 || (int)parcel.Priority == priorityChosen)
                    &&(dateChosen == default(SelectedDatesCollection) || inDate(parcel));
            }
            return false;
        }

        /// <summary>
        /// show the AddParcel window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second RoutedEventArgs type</param>
        private void ButtonView(object sender, RoutedEventArgs e)
        {
            new ParcelView(bl).Show();
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
        /// show the ParcelView window
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel p = new Parcel();
            p.Id = ((PO.ParcelForList)ParcelsListView.SelectedItem).Id;
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
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"failed to update the details of the chosen parcel, {ex.Message}");
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
        /// show the combobox which the user asked to see and collapse the others combobox
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second MouseButtonEventArgs type</param>
        private void ButtonSelect(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == StatusSelectorButton)
            {
                StatusSelector.Visibility = Visibility.Visible;
                WeightSelector.Visibility = Visibility.Collapsed;
                MyCalender.Visibility = Visibility.Collapsed;
                PrioritySelector.Visibility = Visibility.Collapsed;
            }
            if (e.Source == WeightSelectorButton)
            {
                WeightSelector.Visibility = Visibility.Visible;
                StatusSelector.Visibility = Visibility.Collapsed;
                MyCalender.Visibility = Visibility.Collapsed;
                PrioritySelector.Visibility = Visibility.Collapsed;
            }
            if (e.Source == PrioritySelectorButton)
            {
                PrioritySelector.Visibility = Visibility.Visible;
                WeightSelector.Visibility = Visibility.Collapsed;
                MyCalender.Visibility = Visibility.Collapsed;
                StatusSelector.Visibility = Visibility.Collapsed;
            }
            if(e.Source == DateSelectorButton)
            {
                MyCalender.Visibility = Visibility.Visible;
                PrioritySelector.Visibility = Visibility.Collapsed;
                WeightSelector.Visibility = Visibility.Collapsed;
                StatusSelector.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// show only part of the list of the parcels according the request of the user
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
                if(e.Source == WeightSelector)
                {
                    weightChosen = (int)WeightSelector.SelectedItem;
                }
                else
                {
                    priorityChosen = (int)PrioritySelector.SelectedItem;
                }
            }
            try
            {
                parcelsModel.RefreShParcels();
            }
            catch(DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of parcels, {ex.Message}"); }
            catch(UnextantException ex) {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                        $"the details of the parcels");
            }
            catch(XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// check if the parcel in the chosen date/s
        /// </summary>
        /// <param name="item">first PO.ParcelForList type</param>
        /// <returns>bool</returns>
        private bool inDate(PO.ParcelForList item)
        {
            BO.Parcel p = new Parcel() { Id = item.Id };
            try
            {
                lock (bl)
                {
                    p = bl.RequestParcel(p);
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
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.DiscrepanciesException ex)
            {
                MessageBox.Show($"failed to update the details of the chosen parcel, {ex.Message}");
            }
            if (statusChosen == -1)
            {
                if (dateChosen.Count == 1)
                {
                    return (p.Scheduled != null && p.Scheduled >= dateChosen[0] && p.Scheduled <= dateChosen[0].AddDays(1))
                        || (p.Created != null && p.Created >= dateChosen[0] && p.Created <= dateChosen[0].AddDays(1))
                        || (p.PickedUp != null && p.PickedUp >= dateChosen[0] && p.PickedUp <= dateChosen[0].AddDays(1))
                        || (p.Delivered != null && p.Delivered >= dateChosen[0] && p.Delivered <= dateChosen[0].AddDays(1));
                }
                else
                {
                    int index = dateChosen.Count - 1;
                    return (p.Scheduled != null && p.Scheduled >= dateChosen[0] && p.Scheduled <= dateChosen[index].AddDays(1))
                        || (p.Created != null && p.Created >= dateChosen[0] && p.Created <= dateChosen[index].AddDays(1))
                        || (p.PickedUp != null && p.PickedUp >= dateChosen[0] && p.PickedUp <= dateChosen[index].AddDays(1))
                        || (p.Delivered != null && p.Delivered >= dateChosen[0] && p.Delivered <= dateChosen[index].AddDays(1));
                }
            }
            else
            {
                if (dateChosen.Count == 1)
                {
                    switch (statusChosen)
                    {
                        case 0:
                            {
                                return (p.Created >= dateChosen[0] && p.Created <= dateChosen[0].AddDays(1));
                            }
                        case 1:
                            {
                                return (p.Scheduled >= dateChosen[0] && p.Scheduled <= dateChosen[0].AddDays(1));
                            }
                        case 2:
                            {
                                return (p.PickedUp >= dateChosen[0] && p.PickedUp <= dateChosen[0].AddDays(1));
                            }
                        case 3:
                            {
                                return (p.Delivered >= dateChosen[0] && p.Delivered <= dateChosen[0].AddDays(1));
                            }
                        default:
                            break;
                    }
                }
                else
                {
                    int index = dateChosen.Count - 1;
                    switch (statusChosen)
                    {
                        case 0:
                            {
                                return (p.Created >= dateChosen[0] && p.Created <= dateChosen[index].AddDays(1));
                            }
                        case 1:
                            {
                                return (p.Scheduled >= dateChosen[0] && p.Scheduled <= dateChosen[index].AddDays(1));
                            }
                        case 2:
                            {
                                return (p.PickedUp >= dateChosen[0] && p.PickedUp <= dateChosen[index].AddDays(1));
                            }
                        case 3:
                            {
                                return (p.Delivered >= dateChosen[0] && p.Delivered <= dateChosen[index].AddDays(1));
                            }
                        default:
                            break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// visible MyCalender and RefreShParcels by the user chosen if his chosen is valid
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">second SelectionChangedEventArgs type</param>
        private void Selector_Dates(object sender, SelectionChangedEventArgs e)
        {
            MyCalender.Visibility = Visibility.Collapsed;
            SelectedDatesCollection dates = MyCalender.SelectedDates;
            if(dates[0].DayOfWeek != DayOfWeek.Saturday)
            {
                dateChosen = dates;
                try
                {
                    parcelsModel.RefreShParcels();
                }
                catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the parcels, {ex.Message}"); }
                catch(UnextantException ex) {
                        MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                            $"the details of the parcels");
                }
                catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("We do not work on Shabbat", "warning", button, icon);
            }
        }

        /// <summary>
        /// Group Parcels list By SenderName
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void GroupBySender(object sender, RoutedEventArgs e)
        {
            parcelsModel.ParcelView.GroupDescriptions.Clear();
            parcelsModel.ParcelView.GroupDescriptions.Add(new PropertyGroupDescription("SenderName"));
        }

        /// <summary>
        /// Group Parcels list By TargetName
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void GroupByTarget(object sender, RoutedEventArgs e)
        {
            parcelsModel.ParcelView.GroupDescriptions.Clear();
            parcelsModel.ParcelView.GroupDescriptions.Add(new PropertyGroupDescription("TargetName"));
        }

        /// <summary>
        /// cancels the Grouping of parcels list and cancels the previous sort
        /// </summary>
        /// <param name="sender">first object type</param>
        /// <param name="e">secondn RoutedEventArgs type</param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            dateChosen = null;
            statusChosen = -1;
            weightChosen = -1;
            priorityChosen = -1;
            parcelsModel.ParcelView.GroupDescriptions.Clear();
            try
            {
                parcelsModel.RefreShParcels();
            }
            catch (DiscrepanciesException ex) { MessageBox.Show($"failed to refresh the details of the parcels, {ex.Message}"); }
            catch (UnextantException ex) {
                    MessageBox.Show($"the {ex.Message} was not found in the data system and it was needy in order to refresh " +
                        $"the details of the parcels");
            }
            catch (XMLFileLoadCreateException ex) { MessageBox.Show(ex.Message); }
        }
    }
}
