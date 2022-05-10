using BO;
using DalApi;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// the func Create a customer
        /// </summary>
        /// <param name="blCustomer">the first customer object,new params in the Customer object for creating </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(Customer blCustomer)
        {
            DO.Customer dalCustomer = new DO.Customer();
            dalCustomer.Id = blCustomer.Id;
            dalCustomer.Name = blCustomer.Name;
            dalCustomer.Phone = blCustomer.PhoneNumber;
            dalCustomer.Latitude = blCustomer.Location.Latitude;
            dalCustomer.Longitude = blCustomer.Location.Longitude;
            try
            {
                lock (dal)
                {
                    dal.CreateCustomer(dalCustomer);
                }
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("customer", e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        /// <summary>
        /// the func Updates name or/and phone number of a customer
        /// </summary>
        /// <param name="c">the first customer object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer c)
        {
            DO.Customer newCustomer = new DO.Customer
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.PhoneNumber
            };
            try
            {
                lock (dal)
                {
                    dal.UpdateCustomer(newCustomer);
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        #region requestMethods

        /// <summary>
        /// the func requests a customer from the dal
        /// </summary>
        /// <param name="blCustomer">the first customer object</param>
        /// <returns>customer object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer RequestCustomer(Customer blCustomer)
        {
            try
            {
                lock (dal)
                {
                    DO.Customer customer = new DO.Customer();
                    customer = dal.RequestCustomer(blCustomer.Id);
                    BO.Customer c = convertToCustomer(customer);
                    return c;
                }
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// the func requests the list of the customers from the dal
        /// </summary>
        /// <returns>IEnumerable of CustomerForList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerForList> RequestListCustomers()
        {
            List<CustomerForList> customers = new List<CustomerForList>();
            try
            {
                IEnumerable<DO.Customer> temp = Enumerable.Empty<DO.Customer>();
                temp = dal.RequestListCustomers();
                lock (dal)
                {
                    foreach (DO.Customer item in temp)
                    {
                        CustomerForList c = new CustomerForList { Id = item.Id, Name = item.Name, PhoneNumber = item.Phone, AcceptedPackages = 0, DeliveredPackages = 0, PackagesInWay = 0, SendedPackages = 0 };
                        IEnumerable<DO.Parcel> senderDeliveredParcels = dal.RequestPartListParcels(p => p.SenderId == item.Id && p.Delivered != null);
                        IEnumerable<DO.Parcel> senderScheduledParcels = dal.RequestPartListParcels(p => p.SenderId == item.Id && p.Scheduled != null);
                        IEnumerable<DO.Parcel> targetDeliveredParcels = dal.RequestPartListParcels(p => p.TargetId == item.Id && p.Delivered != null);
                        IEnumerable<DO.Parcel> targetPickedUpParcels = dal.RequestPartListParcels(p => p.TargetId == item.Id && p.PickedUp != null);
                        c.DeliveredPackages = senderDeliveredParcels.Count();
                        c.SendedPackages = senderScheduledParcels.Count();
                        c.AcceptedPackages = targetDeliveredParcels.Count();
                        c.PackagesInWay = targetPickedUpParcels.Count();
                        customers.Add(c);
                    }
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            return customers.OrderBy(c=>c.Id);
        }
        #endregion

        /// <summary>
        /// delete customer if the it can be deleted
        /// </summary>
        /// <param name="id">first int value</param>
        /// <returns>bool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteCustomer(int id)
        {
            Customer boC = new Customer() { Id = id };
            try
            {
                boC = RequestCustomer(boC);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            bool temp=true;
            temp= boC.ToCustomer.Where(item => item.Status != (Enum.DeliveryStatus)3).Select(p => false).FirstOrDefault();
            if (!temp) return temp;
            temp= boC.FromCustomer.Where(item => item.Status == (Enum.DeliveryStatus)0 || item.Status == (Enum.DeliveryStatus)1 || item.Status == (Enum.DeliveryStatus)2).Select(p => false).FirstOrDefault();
            if (!temp) return temp;
            try
            {
                lock (dal)
                {
                    dal.DeleteBaseStation(id);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            return true;
        }
    }
}
