using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class BaseStation:ILocate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; } = new Location();
        public int AvailableChargeSlots { get; set; }
        public List<DroneInCharging> DronesInCharching = new List<DroneInCharging>();

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            string str = "";
            foreach (var item in DronesInCharching)
            {
                str += item.Id.ToString() + " ";
            }
            return $"base-station id: {Id} name: {Name} location: {Location}, " +
                $"available chargeSlots: {AvailableChargeSlots}, Drones in charging: {str}";
        }
    }
}

