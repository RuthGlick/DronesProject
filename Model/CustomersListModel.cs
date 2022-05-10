using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PO;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace PL.Model
{
    public class CustomersListModel: DependencyObject
    {
        static readonly CustomersListModel instance = new CustomersListModel();
        internal static CustomersListModel Instance { get => instance; }

        BlApi.IBL bl;
        ListCollectionView customerView;

        /// <summary>
        /// init bl, and 2 lists
        /// </summary>
        private CustomersListModel()
        {
            try
            {
                bl = BlApi.BlFactory.GetBl();
                lock (bl)
                {
                    Customers = new ObservableCollection<CustomerForList>();
                    foreach (var item in bl.RequestListCustomers())
                        Customers.Add(convertCustomer(item));
                }
            }
            catch (BO.BLConfigException e)
            {
                throw new PO.BLConfigException(e.Message, e);
            }
            catch (BO.DiscrepanciesException e)
            {
                throw new PO.DiscrepanciesException(e.Message, e);
            }
            catch (BO.UnextantException e)
            {
                throw new PO.UnextantException(e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw new PO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }


        /// <summary>
        /// convert CustomerForList BO to PO
        /// </summary>
        /// <param name="item">first BO.CustomerForList object</param>
        /// <returns>PO.CustomerForList</returns>
        private PO.CustomerForList convertCustomer(BO.CustomerForList item)
        {
            PO.CustomerForList customerConverted = new PO.CustomerForList()
            {
                Id = item.Id,
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                DeliveredPackages = item.DeliveredPackages,
                SendedPackages = item.SendedPackages,
                AcceptedPackages = item.AcceptedPackages,
                PackagesInWay = item.PackagesInWay
            };
            return customerConverted;
        }

        public readonly DependencyProperty listCustomers =
          DependencyProperty.Register(nameof(Customers), typeof(ObservableCollection<PO.CustomerForList>), typeof(CustomersListModel), new PropertyMetadata(null));

        public ObservableCollection<PO.CustomerForList> Customers
        {
            get { return (ObservableCollection<PO.CustomerForList>)GetValue(listCustomers); }
            private set { SetValue(listCustomers, value); }
        }

        /// <summary>
        /// RefreSh the 2 lists 
        /// </summary>
        public void RefreShCustomers()
        {
            Customers.Clear();
            try
            {
                lock (bl)
                {
                    foreach (var item in bl.RequestListCustomers())
                        Customers.Add(convertCustomer(item));
                }
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw new PO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        public ListCollectionView BaseStationView
        {
            get => customerView;
            set => customerView = value;
        }
    }
}
