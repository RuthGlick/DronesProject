using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// the func Create a Drone
        /// </summary>
        /// <param name="blDrone">the first drone object,new params in the Drone object for creating </param>
        /// <param name="idStation">the second int value,Station number - Put the skimmer in it for initial charging </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateDrone(Drone blDrone, int idStation)
        {
            DO.BaseStation station = new DO.BaseStation();
            try
            {
                lock (dal)
                {
                    station = dal.RequestBaseStation(idStation);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            blDrone.Battery = ran.Next(20, 41);
            blDrone.Status = DroneStatuses.MAINTENANCE;
            blDrone.Location = new Location();
            blDrone.Location.Latitude = station.Latitude;
            blDrone.Location.Longitude = station.Longitude;
            blDrone.DeliveryByTransfer = null;
            try
            {
                lock (dal)
                {
                    dal.CreateChargeSlot(new DO.DroneCharge { DroneId = blDrone.Id, StationId = idStation });
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("charge slot", e);
            }
            DO.Drone dalDrone = new DO.Drone();
            dalDrone.Id = blDrone.Id;
            dalDrone.Model = blDrone.Model;
            dalDrone.MaxWeight = (DO.WeightCategories)blDrone.Weight;
            try
            {
                lock (dal)
                {
                    dal.CreateDrone(dalDrone);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("drone", e);
            }

            DroneForList droneForList = new DroneForList();
            droneForList.Id = blDrone.Id;
            droneForList.Model = blDrone.Model;
            droneForList.Weight = blDrone.Weight;
            droneForList.Battery = blDrone.Battery;
            droneForList.Status = blDrone.Status;
            droneForList.Location = new Location() { Longitude = blDrone.Location.Latitude, Latitude = blDrone.Location.Latitude };
            droneForList.DeliveryId = -1;
            drones.Add(droneForList);
        }

        #region updateMethods

        /// <summary>
        /// the func releases a drone from charging
        /// </summary>
        /// <param name="d">the first DroneInCharging object</param>
        /// <param name="time">the second unt value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateRelease(DroneInCharging d, int time)
        {
            DroneForList drone = drones.Find((drone1) => drone1.Id == d.Id);
            if (drone == null)
            {
                throw new UnextantException("drone");
            }
            if (drone.Status != (DroneStatuses)1)
            {
                throw new DiscrepanciesException("you can't release a drone which isn't in charging");
            }
            drones.Remove(drone);
            d.BatteryStatus = drone.Battery + calculateBattery(time);
            if (d.BatteryStatus > 100)
            {
                d.BatteryStatus = 100;
            }
            drone.Battery = d.BatteryStatus;
            drone.Status = (DroneStatuses)0;
            drones.Add(drone);
            DO.DroneCharge droneCharge = new DO.DroneCharge();
            droneCharge.DroneId = drone.Id;
            try
            {
                lock (dal)
                {
                    dal.UpdateRelease(droneCharge);
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        /// <summary>
        /// the func Updates model of a drone
        /// </summary>
        /// <param name="newBlDrone">the first Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone newBlDrone)
        {
            DO.Drone drone = new DO.Drone();
            try
            {
                lock (dal)
                {
                    drone = dal.RequestDrone(newBlDrone.Id);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            DroneForList d = drones.Find((drone1) => drone1.Id == drone.Id);
            d.Status = DroneStatuses.AVAILABLE;
            d.Model = newBlDrone.Model;
            try
            {
                lock (dal)
                {
                    dal.UpdateDrone(drone, newBlDrone.Model);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        /// <summary>
        /// the func sends a drone to charging 
        /// </summary>
        /// <param name="d">the first Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCharge(Drone d)
        {
            Drone drone = new Drone();
            DroneForList droneForList = drones.FirstOrDefault(item => d.Id == item.Id);
            if (drone == default(Drone))
            {
                throw new UnextantException("drone");
            }
            drones.Remove(droneForList);
            try
            {
                drone = convertToDrone(droneForList);
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }

            if (drone.Status == DroneStatuses.TRANSPORT)
            {
                throw new DiscrepanciesException("you can't charge a not available drone");
            }

            BaseStation b = new BaseStation();
            BaseStation nearBS = new BaseStation();

            List<BaseStation> blBaseStations = new List<BaseStation>();
            try
            {
                lock (dal)
                {
                    blBaseStations = (dal.RequestListBaseStations().Select(item => convertDalBaseStationToBL(item))).ToList<BaseStation>();
                }
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }

            double minDistance = blBaseStations[0].DistanceTo(drone);
            nearBS = blBaseStations[0];
            int index = 0;
            bool boolEnough;
            try
            {
                boolEnough = enough(minDistance, drone.Battery, 0);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            while (blBaseStations[index].AvailableChargeSlots == 0 || !boolEnough)
            {
                if (++index == blBaseStations.Count)
                {
                    throw new UnextantException("base station");
                }
                minDistance = blBaseStations[index].DistanceTo(d);
                nearBS = blBaseStations[index];
            }
            for (int i = ++index; i < blBaseStations.Count; ++i)
            {
                try
                {
                    boolEnough = enough(minDistance, drone.Battery, 0);
                }
                catch (XMLFileLoadCreateException e)
                {
                    throw e;
                }
                catch (DiscrepanciesException e)
                {
                    throw e;
                }
                if (blBaseStations[i].AvailableChargeSlots > 0 && boolEnough && blBaseStations[i].DistanceTo(d) < minDistance)
                {
                    nearBS = blBaseStations[i];
                    minDistance = blBaseStations[i].DistanceTo(drone);
                }
            }
            try
            {
                lock (dal)
                {
                    dal.UpdateCharge(new DO.DroneCharge { StationId = nearBS.Id, DroneId = d.Id });
                }
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("drone charge", e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            nearBS.AvailableChargeSlots--;
            drone.Battery -= ((int)minBattery(minDistance, 0) + 1);
            drone.Location = new Location() { Longitude = nearBS.Location.Longitude, Latitude = nearBS.Location.Latitude };
            droneForList.Location = new Location() { Longitude = nearBS.Location.Longitude, Latitude = nearBS.Location.Latitude };
            drone.Status = DroneStatuses.MAINTENANCE;
            droneForList.Status = DroneStatuses.MAINTENANCE;
            drones.Add(droneForList);
        }

        /// <summary>
        /// the func belongs a drone to a parcel
        /// </summary>
        /// <param name="d">the first Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateScheduled(Drone d)
        {
            Drone drone = new Drone();
            DroneForList droneForList = drones.Find(item => d.Id == item.Id);
            if (drone == null || drone.Status != DroneStatuses.AVAILABLE)
            {
                throw new UnextantException("drone");
            }
            try
            {
                drone = convertToDrone(droneForList);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            List<ParcelForList> optionalParcels = new List<ParcelForList>();
            IEnumerable<DO.Parcel> list = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    list = dal.RequestPartListParcels(p => p.Scheduled == null);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            try
            {
                optionalParcels = (from item in list
                                   let distance = drone.DistanceTo(findCustomer(item.SenderId))
                                                       + findCustomer(item.SenderId).DistanceTo(findCustomer(item.TargetId))
                                                       + nearStation(drone).DistanceTo(drone)
                                   where enough(distance, drone.Battery, (int)item.Weight + 1) && (WeightCategories)item.Weight <= drone.Weight
                                   select convertParcel(item)).ToList<ParcelForList>();
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }

            if (optionalParcels.Count == 0)
            {
                throw new UnextantException("parcel");
            }
            int chosenId;
            try
            {
                chosenId = findParcel(optionalParcels, drone);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            DO.Parcel p = new DO.Parcel { Id = chosenId, DroneId = drone.Id, Scheduled = DateTime.Now };
            try
            {
                lock (dal)
                {
                    dal.UpdateScheduled(p);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.DiscrepanciesException e)
            {
                throw new DiscrepanciesException("failed to schedule a parcel to the drone", e);
            }
            DroneForList droneFor = drones.FirstOrDefault(d => d.Id == drone.Id);
            if(droneFor == null)
            {
                throw new UnextantException("drone");
            }
            drones.Remove(droneFor);
            droneFor.DeliveryId = chosenId;
            droneFor.Status = DroneStatuses.TRANSPORT;
            drones.Add(droneFor);
        }

        /// <summary>
        /// the func picks up a parcel by a drone
        /// </summary>
        /// <param name="d">the first Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePickedUp(Drone d)
        {
            DO.Drone drone = new DO.Drone();
            try
            {
                lock (dal)
                {
                    drone = dal.RequestDrone(d.Id);
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }

            DroneForList droneForList = new DroneForList();
            int senderId;
            int parcelId;
            Customer sender = new Customer();
            if (!checkForPickedUp(d, out droneForList, out senderId, out parcelId))
            {
                throw new DiscrepanciesException("the parcel is not available");
            }
            DO.Customer item = new DO.Customer();
            try
            {
                lock (dal)
                {
                    item = dal.RequestCustomer(senderId);
                    sender = convertToCustomer(item);
                }
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            drones.Remove(droneForList);
            double distance = droneForList.DistanceTo(sender);
            droneForList.Battery -= ((int)minBattery(distance, 0) + 1);
            droneForList.Location = new Location() { Longitude = sender.Location.Latitude, Latitude = sender.Location.Latitude };
            drones.Add(droneForList);
            DO.Parcel parcel = new DO.Parcel();
            parcel.Id = parcelId;
            dal.UpdatePickedUp(parcel);
        }

        /// <summary>
        /// the func provides a parcel by a drone
        /// </summary>
        /// <param name="d">the first Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSupply(Drone d)
        {
            DroneForList droneForList = drones.FirstOrDefault(drone => drone.Id == d.Id);
            if(droneForList == null)
            {
                throw new UnextantException("drone");
            }
            if (droneForList.DeliveryId ==0)
            {
                throw new DiscrepanciesException("failed to supply the parcel");
            }

            DO.Parcel p = new DO.Parcel();
            try
            {
                lock (dal)
                {
                    p = dal.RequestParcel(droneForList.DeliveryId);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            if (p.PickedUp == null || p.Delivered != null)
            {
                throw new DiscrepanciesException("fail to supply the parcel");
            }
            Customer c = new Customer();
            try
            {
                c = findCustomer(p.TargetId);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            drones.Remove(droneForList);
            droneForList.Battery -= ((int)minBattery(droneForList.DistanceTo(c), (int)p.Weight + 1) + 1);
            droneForList.Location = new Location() { Longitude = c.Location.Longitude, Latitude = c.Location.Latitude };
            p.Delivered = DateTime.Now;
            try
            {
                lock (dal)
                {
                    dal.UpdateSupply(p);
                }
            }
            catch (DO.DiscrepanciesException e)
            {
                throw new DiscrepanciesException("failed to supply the parcel", e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            droneForList.DeliveryId = 0;
            droneForList.Status = DroneStatuses.AVAILABLE;
            drones.Add(droneForList);
        }

        #endregion

        #region requestMethods

        /// <summary>
        /// the func requests a drone from the dal
        /// </summary>
        /// <param name="blDrone">the first Drone object</param>
        /// <returns> Drone object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone RequestDrone(Drone blDrone)
        {
            DroneForList droneForList = new DroneForList();
            droneForList = drones.Find(drone1 => drone1.Id == blDrone.Id);
            if (droneForList == null)
            {
                throw new UnextantException("drone");
            }
            try
            {
                return convertToDrone(droneForList);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// the func requests the list of the drones from the dal
        /// </summary>
        /// <returns>IEnumerable of DroneForList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneForList> RequestListDrones()
        {
            return drones.Select(drone => drone.Clone(drone)).OrderBy(d=>d.Id);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item</param>
        /// <returns>ienumerable of drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneForList> RequestPartListDrones(Predicate<DroneForList> predicate)
        {
            return drones.Where(drone => predicate(drone)).OrderBy(d => d.Id);
        }
        #endregion
        /// <summary>
        /// delete Drone if the it can be deleted
        /// </summary>
        /// <param name="id">first int value</param>
        /// <returns>bool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteDrone(int id)
        {
            Drone boDrone = new Drone() { Id = id };
            try
            {
                boDrone = RequestDrone(boDrone);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            if (boDrone.Status==(Enum.DroneStatuses)2)
            {
                return false;
            }
            if (boDrone.Status == Enum.DroneStatuses.MAINTENANCE)
            {
                lock (dal)
                {
                    DO.DroneCharge d = dal.RequestDroneCharges().FirstOrDefault(item => item.DroneId == boDrone.Id);
                    if (d.DroneId != 0)
                    {
                        dal.UpdateRelease(d);
                    }
                }
            }
            try
            {
                lock (dal)
                {
                    dal.DeleteDrone(id);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }  
            DroneForList droneForList = new DroneForList();
            droneForList = drones.Where(drone => drone.Id == id).FirstOrDefault();
            if(droneForList != null)
            {
                drones.Remove(droneForList);
                return true;
            }
            return false;
        }
    }
}
