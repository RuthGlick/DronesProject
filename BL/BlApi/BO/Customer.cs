using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Customer:ILocate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Location Location { get; set; } = new Location();

        public List<ParcelToCustomer> FromCustomer = new List<ParcelToCustomer>();
        public List<ParcelToCustomer> ToCustomer = new List<ParcelToCustomer>();

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            string fromCustomer = "";
            foreach (var item in FromCustomer)
            {
                fromCustomer += item.Id.ToString() + " ";
            }
            string toCustomer = "";
            foreach (var item in ToCustomer)
            {
                toCustomer += item.Id.ToString() + " ";
            }
            return $"customer - id: {Id}, name: {Name}, phoneNumber: {PhoneNumber}, " +
                $"location: {Location.Longitude}, {Location.Latitude}, fromCustomer: {fromCustomer}, toCustomer: {toCustomer}";
        }

        
    }
}

