using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;


namespace BO
{
    public class Drone:ILocate
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public ParcelInTransfer DeliveryByTransfer { get; set; }
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"drone id: {Id} drone model: {Model}, max weight: {Weight}, " +
                $"battery: {Battery}, status: {Status}, delivery by transfer: {DeliveryByTransfer}" +
                $"location: {Location}";
        }
    }
}

