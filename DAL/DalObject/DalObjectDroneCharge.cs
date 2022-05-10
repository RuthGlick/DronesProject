using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using static DAL.DataSource;
using static DAL.DataSource.Confing;

namespace DAL
{
    internal partial class DalObject
    {
        /// <summary>
        /// the method returns List of the Drone Charge
        /// </summary>
        /// <returns>List of DroneCharge objects</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<DroneCharge> RequestDroneCharges()
        {
            return DroneCharges;
        }

        /// <summary>
        /// the method Create a new Charge Slot 
        /// </summary>
        /// <param name="droneCharge">DroneCharge object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateChargeSlot(DroneCharge droneCharge)
        {
            if (DroneCharges.Contains(droneCharge))
            {
                throw new ExtantException("drone charge");
            }
            DroneCharges.Add(droneCharge);
            DroneCharges.Sort();
        }

        /// <summary>
        /// UpdateRelease is a method in the DalObject class.
        /// the method removes a drone from charging at a base station
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateRelease(DroneCharge d)
        {
            bool isDroneInCharge = false;
            DroneCharge droneCharge = DroneCharges.FirstOrDefault(dc => dc.DroneId == d.DroneId);
            if(droneCharge.DroneId != 0)
            {
                isDroneInCharge = true;
                DroneCharges.Remove((DroneCharge)droneCharge);
            }
            if(!isDroneInCharge)
            {
                throw new UnextantException("drone charge");
            }
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of drone charge</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> RequestPartListDroneCharges(Predicate<DroneCharge> predicate)
        {
            return DroneCharges.Where(droneCharge => predicate(droneCharge));
        }
    }
}