using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;


namespace BO
{
    public class ParcelForList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DeliveryStatus Status { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"parcel to list -  id: {Id}, sender: {SenderName}, target: {TargetName}" +
                $"weight: {Weight}, priority: {Priority} status: {Status}";
        }
    }
}

