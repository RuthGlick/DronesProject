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
        List<ParcelToCustomer> FromCustomer = new List<ParcelToCustomer>();
        List<ParcelToCustomer> ToCustomer = new List<ParcelToCustomer>();

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"customer - id: {Id}, name: {Name}, phoneNumber: {PhoneNumber}, location: {Location.Longitude}, {Location.Latitude}";
        }

        
    }
}

