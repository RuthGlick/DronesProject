using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStationForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargeSlots { get; set; }
        public int CatchChargeSlots { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"baseStation to list - id: {Id}, name: {Name}, availableChargeSlots: {AvailableChargeSlots}, catchChargeSlots: {CatchChargeSlots}";
        }
    }
}
