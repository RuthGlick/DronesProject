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
        static Random ran = new Random();

        /// <summary>
        /// AddDrone is a method in the DalObject class.
        /// the method adds a new drone
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateDrone(Drone drone)
        {
            Drone exist = DronesList.FirstOrDefault(d => d.Id == drone.Id && d.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("drone");
            }
            drone.IsDeleted = false;
            DronesList.Add(drone);
        }

        /// <summary>
        /// UpdateCharge is a method in the DalObject class.
        /// the method updates sending a skimmer for charging at a base station
        /// </summary>
        /// <param name="dc">DroneCharge onject</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCharge(DroneCharge dc)
        {
            try
            {
                CreateChargeSlot(dc);
            }
            catch (ExtantException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// DisplayDrone is a method in the DalObject class.
        /// the method allows drone display
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone,string newModel)
        {
            Drone temp;
            try
            {
                temp = RequestDrone(drone.Id);
            }
            catch(UnextantException e) { throw e; }
            DronesList.Remove(temp);
            temp.Model = newModel;
            DronesList.Add(temp);
        }

        /// <summary>
        /// the func returns drone by id of drone
        /// </summary>
        /// <param name="id">int value</param>
        /// <returns>drone object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone RequestDrone(int id)
        {
            Drone drone = DronesList.FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (drone.Id == 0)
            {
                throw new UnextantException("drone");
            }
            return (Drone)drone;
        }

        /// <summary>
        /// ViewListDrones is a method in the DalObject class.
        /// the method displays the List of drones
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> RequestListDrones()
        {
            return DronesList.Where(item=> item.IsDeleted == false);
        }

        /// <summary>
        /// the method returns an array of five values according to the data from the Config class, 
        /// in the following order: free, light, medium, heavy and loading rate
        /// </summary>
        /// <returns>array of double values</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] RequestElectricity()
        {
            double[] arr = new double[5];
            arr[0] = Confing.Available;
            arr[1] = Confing.LightWeight;
            arr[2] = Confing.MediumWeight;
            arr[3] = Confing.HeavyWeight;
            arr[4] = Confing.ChargingRate;
            return arr;
        }

        /// <summary>
        /// UpdateScheduled is a method in the DalObject class.
        /// the method assigns a package to the drone
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateScheduled(Parcel p)
        {
            Parcel temp = new Parcel();
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            ParcelsList.Remove(temp);
            temp.Scheduled = p.Scheduled;
            temp.DroneId = p.DroneId;
            ParcelsList.Add(temp);
        }

        /// <summary>
        /// UpdatePickedUp is a method in the DalObject class.
        /// the method updates package assembly by drone
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePickedUp(Parcel p)
        {
            Parcel temp;
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch(UnextantException e) { throw e; }
            ParcelsList.Remove(temp);
            temp.PickedUp = DateTime.Now;
            ParcelsList.Add(temp);
        }

        /// <summary>
        /// UpdateSupply is a method in the DalObject class.
        /// the method updates delivery of a package to the destination
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSupply(Parcel p)
        {
            Parcel temp = new Parcel();
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            ParcelsList.Remove(temp);
            temp.Delivered = p.Delivered;
            ParcelsList.Add(temp);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of Drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> RequestPartListDrones(Predicate<Drone> predicate)
        {
            return DronesList.Where(drone => predicate(drone)&&drone.IsDeleted == false);
        }

        /// <summary>
        /// delete Drone - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            Drone temp;
            try
            {
                temp = RequestDrone(id);
            }
            catch (UnextantException e) { throw e; }
            DronesList.Remove(temp);
            temp.IsDeleted = true;
            DronesList.Add(temp);
        }
    }
}