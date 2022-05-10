using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using static DAL.DataSource;
using static DAL.DataSource.Confing;

namespace DAL
{
    internal partial class DalObject
    {
        /// <summary>
        /// CreateCustomer is a method in the DalObject class.
        /// the method adds a new customer
        /// </summary>
        /// <param name="customer">the first Customer value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(Customer customer)
        {
            Customer exist = CustomersList.FirstOrDefault(c => c.Id == customer.Id && c.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("customer");
            }
            customer.IsDeleted = false;
            CustomersList.Add(customer);
        }

        /// <summary>
        /// DisplayCustomer is a method in the DalObject class.
        /// the method retuurns customer 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer RequestCustomer(int id)
        {
            Customer customer = CustomersList.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (customer.Id == 0)
            {
                throw new UnextantException("customer");
            }
            return (Customer)customer;
        }

        /// <summary>
        /// ViewListParcels is a method in the DalObject class.
        /// the method displays View the customer List
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> RequestListCustomers()
        {
            return CustomersList.Where(item=> item.IsDeleted == false);
        }

        /// <summary>
        /// update Customer object - its name or its phone or the 2 details
        /// </summary>
        /// <param name="c">first Customer value</param>
        public void UpdateCustomer(Customer c)
        {
            Customer temp = new Customer();
            try
            {
                temp = RequestCustomer(c.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            CustomersList.Remove(temp);
            if (c.Name != "")
            {
                temp.Name = c.Name;
            }
            if (c.Phone != "")
            {
                temp.Phone = c.Phone;
            }
            CustomersList.Add(temp);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> RequestPartListCustomers(Predicate<Customer> predicate)
        {
            return CustomersList.Where(customer => predicate(customer) && customer.IsDeleted == false);
        }

        /// <summary>
        /// delete Customer - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {
            Customer temp;
            try
            {
                temp = RequestCustomer(id);
            }
            catch (UnextantException e) { throw e; }
            CustomersList.Remove(temp);
            temp.IsDeleted = true;
            CustomersList.Add(temp);
        }
    }
}
