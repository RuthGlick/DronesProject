using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;

namespace BO
{
    public class ParcelToCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DeliveryStatus Status { get; set; }
        public CustomerInParcel Partner { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"deliveryId: {Id}, weight: {Weight}, priority: {Priority}, " +
                $"status: {Status}, partner: {Partner}";
        }
    }
}

