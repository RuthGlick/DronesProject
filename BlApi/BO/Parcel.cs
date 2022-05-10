using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel Drone1 { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"parcel id: {Id} sender: {Sender} target: {Target}" +
                $"weight: {Weight}, priority: {Priority}, drone: {Drone1}" +
                $"time of creating: {Created}, time of scheduling: {Scheduled}" +
                $"time of picking up: {PickedUp}, time of delivering: {Delivered}";
        }
    }
}
