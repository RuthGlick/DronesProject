using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;


namespace BO
{
    public class ParcelInTransfer:ILocate
    {
        public int Id { get; set; }
        public bool status { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; } = new CustomerInParcel();
        public CustomerInParcel Target { get; set; } = new CustomerInParcel();
        public Location Location { get; set; } = new Location();
        public Location Destination { get; set; } = new Location();
        public double TransportDistance { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"packageId: {Id}, status: {status}, priority: {Priority}," +
                $"weight: {Weight}, sender: {Sender}, target: {Target}" +
                $"sourse: {Location}, destination: {Destination}, transportDistance: {TransportDistance}";
        }
    }
}
