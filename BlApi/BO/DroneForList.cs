using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;


namespace BO
{
    public class DroneForList:ILocate 
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public Location Location { get; set; } = new Location();
        public int DeliveryId { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"Id: {Id}-----Model: {Model}-----MaxWeight: {Weight}-----" +
                $"Battery: {Battery}-----Status: {Status}-----Location: {Location}-----" +
                $"DeliveryId: {DeliveryId}";
        }

        public DroneForList Clone(DroneForList drone)
        {
            return new DroneForList()
            {
                Id = drone.Id,
                Model = drone.Model,
                Weight = drone.Weight,
                Status = drone.Status,
                Location = drone.Location,
                Battery = drone.Battery,
                DeliveryId = drone.DeliveryId
            };
        }
    }
}

