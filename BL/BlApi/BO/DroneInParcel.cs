using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double BatteryStatus { get; set; }
        public Location Current { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"droneId: {Id}, BatteryStatus: {BatteryStatus}, current location: {Current}";
        }
    }
    
}
