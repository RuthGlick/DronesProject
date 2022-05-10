using PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace PL.Model
{
    public class CustomerModel : DependencyObject, INotifyPropertyChanged
    {
        static List<CustomerModel> listWindows = new List<CustomerModel>();

        /// <summary>
        /// create only one instance of myCustomer
        /// </summary>
        /// <param name="customer">the first BO.Customer value</param>
        /// <returns>CustomerModel value</returns>
        public CustomerModel GetCustomerModel(BO.Customer customer)
        {
            CustomerModel exist = (listWindows.Where(c => c.myCustomer.Id == customer.Id).Select(c => c)).FirstOrDefault();
            if (exist != null)
            {
                return exist;
            }
            MyCustomer = convertToPO(customer);
            listWindows.Add(this);
            return this;
        }

        private PO.Customer myCustomer;
        public PO.Customer MyCustomer
        {
            get => myCustomer;
            set
            {
                myCustomer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyCustomer)));
            }
        }

        private bool isAdd;
        public bool IsAdd
        {
            get => isAdd;
            set
            {
                isAdd = value;
                isView = !isAdd;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdd)));
            }
        }

        private bool isView;
        public bool IsView
        {
            get => isView;
            set
            {
                isView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsView)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// convert from BO to PO Customer
        /// </summary>
        /// <param name="customer">the first BO.Customer value</param>
        /// <returns>Customer value</returns>
        private Customer convertToPO(BO.Customer customer)
        {
            if (customer == null) { return new Customer(); }
            Customer c = new Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber
            };
            if (customer.Location != null) { c.Location = new Location() { Longitude = customer.Location.Longitude, Latitude = customer.Location.Latitude }; }
            if (customer.FromCustomer.Count > 0)
            {
                foreach (var item in customer.FromCustomer)
                {
                    c.ofromCustomer.Add(new ParcelToCustomer()
                    {
                        Id = item.Id,
                        Partner = new CustomerInParcel()
                        { Id = item.Partner.Id, Name = item.Partner.Name },
                        Priority = (PO.Enum.Priorities)item.Priority,
                        Status = (PO.Enum.DroneStatuses)item.Status,
                        Weight = (PO.Enum.WeightCategories)item.Weight
                    });
                }
            }
            if (customer.ToCustomer.Count > 0)
            {
                foreach (var item in customer.ToCustomer)
                {
                    c.otoCustomer.Add(new ParcelToCustomer()
                    {
                        Id = item.Id,
                        Partner = new CustomerInParcel()
                        { Id = item.Partner.Id, Name = item.Partner.Name },
                        Priority = (PO.Enum.Priorities)item.Priority,
                        Status = (PO.Enum.DroneStatuses)item.Status,
                        Weight = (PO.Enum.WeightCategories)item.Weight
                    });
                }
            }
            return c;
        
    }
        }
    }

