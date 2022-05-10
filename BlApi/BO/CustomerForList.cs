using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class CustomerForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int DeliveredPackages { get; set; }
        public int SendedPackages { get; set; }
        public int AcceptedPackages { get; set; }
        public int PackagesInWay { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"customer in list - id: {Id}, name: {Name}, phoneNumber: {PhoneNumber}" +
                $"delivered packages: {DeliveredPackages}, sended packages: {SendedPackages}, " +
                $"accepted packages: {AcceptedPackages}, packages in way: {PackagesInWay}";
        }
    
    }
}
