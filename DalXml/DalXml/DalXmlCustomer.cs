using System.Runtime.CompilerServices;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    internal partial class DalXml
    {
        /// <summary>
        /// CreateCustomer is a method in the DalXml class.
        /// the method adds a new customer
        /// </summary>
        /// <param name="customer">the first Customer value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(Customer customer)
        {
            customer.IsDeleted = false;
            List<Customer> customersXml;
            try
            {
                customersXml = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch(XMLFileLoadCreateException e) { throw e; }
            Customer exist = customersXml.FirstOrDefault(c => c.Id == customer.Id && c.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("customer");
            }
            customersXml.Add(customer);
            try
            {
                XMLTools.SaveListToXmlSerializer<Customer>(customersXml, customersPath);
            }
            catch(XMLFileLoadCreateException e) { throw e; }
        }
      
        #region requestMethods

        /// <summary>
        /// RequestListCustomers is a method in the DalXml class.
        /// the method returns the customer List
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> RequestListCustomers()
        {
            List<Customer> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(item => item.IsDeleted == false);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> RequestPartListCustomers(Predicate<Customer> predicate)
        {
            List<Customer> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(customer => predicate(customer) && customer.IsDeleted == false);
        }
        
        /// <summary>
        /// the func returns customer by id of customer
        /// </summary>
        /// <param name="id">int value</param>
        /// <returns>customer object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer RequestCustomer(int id)
        {
            List<Customer> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            Customer customer = list.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if(customer.Id == 0)
            {
                throw new UnextantException("customer");
            }
            return (Customer)customer;
        }
   
        #endregion

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
                temp= RequestCustomer(id);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }

            List<Customer> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            temp.IsDeleted = true;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer<Customer>(list, customersPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// update customer object - its name or its phone or the 2 details
        /// </summary>
        /// <param name="c">the first Customer value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer c)
        {
            Customer temp = new Customer();
            try
            {
                temp = RequestCustomer(c.Id);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            List<Customer> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            if (c.Name != "")
            {
                temp.Name = c.Name;
            }
            if (c.Phone != "")
            {
                temp.Phone = c.Phone;
            }
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer(list, customersPath);
            }
            catch(XMLFileLoadCreateException e) { throw e; }
        }
    }
}
