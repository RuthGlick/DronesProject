using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;
using BO;
using DO;
using BaseStation = BO.BaseStation;
using Customer = BO.Customer;
using Drone = BO.Drone;
using WeightCategories = BL.Enum.WeightCategories;
using Priorities = BL.Enum.Priorities;
using DiscrepanciesException = BO.DiscrepanciesException;
using UnextantException = BO.UnextantException;


namespace BL
{
    partial class BL
    {
        Random ran = new Random();

        #region chargeSlots
       
        /// <summary>
        /// check if the base staion has available charge slots
        /// </summary>
        /// <param name="baseStation">first baseStation object</param>
        /// <returns>bool</returns>
        bool haveAvailableChargeSlots(DO.BaseStation baseStation)
        {
            int sum = baseStation.ChargeSlots;
            List<DO.DroneCharge> list = new List<DroneCharge>();
            try
            {
                lock (dal)
                {
                    list = dal.RequestDroneCharges();
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            sum -= list.Where(item1 => item1.StationId == baseStation.Id).Count();
            if (sum > 0)
                return true;
            return false;
        }

        /// <summary>
        /// the func calculates the sum of catch charge slots of a base station
        /// </summary>
        /// <param name="b">the first DO.BaseStation object</param>
        /// <returns>catchChargeSlots object</returns>
        private int catchChargeSlots(DO.BaseStation b)
        {
            IEnumerable<DO.DroneCharge> droneCharges;
            try
            {
                lock (dal)
                {
                    droneCharges = dal.RequestPartListDroneCharges(droneCharge => droneCharge.StationId == b.Id).ToList();
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            return droneCharges.Count();
        }

        #endregion

        #region station

        /// <summary>
        /// the func randoms a location among the exist base stations
        /// </summary>
        /// <returns>Location object</returns>
        private Location randomStation(out int id)
        {
            IEnumerable<DO.BaseStation> dalBaseStations = Enumerable.Empty<DO.BaseStation>();
            try
            {
                lock (dal)
                {
                    dalBaseStations = dal.RequestListBaseStations();
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            int index = ran.Next(0, dalBaseStations.Count());
            id = dalBaseStations.ElementAt(index).Id;
            return new Location()
            {
                Longitude = dalBaseStations.ElementAt(index).Longitude,
                Latitude = dalBaseStations.ElementAt(index).Latitude
            };
        }

        /// <summary>
        /// the func looks for the nearset base station to a certain location
        /// </summary>
        /// <param name="location">the first ILocate object</param>
        /// <returns>BaseStation object</returns>
        internal BaseStation nearStation(ILocate location)
        {
            IEnumerable<DO.BaseStation> dalBaseStations = Enumerable.Empty<DO.BaseStation>();
            try
            {
                lock (dal)
                {
                    dalBaseStations = dal.RequestListBaseStations();
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            List<BaseStation> baseStations = new List<BaseStation>();
            baseStations = (dalBaseStations.Select(b => convertDalBaseStationToBL(b))).ToList();
            double minDistance = baseStations.ElementAt(0).DistanceTo(location), distance;
            BaseStation nearest = baseStations[0];
            minDistance = baseStations.Min(item => item.DistanceTo(location));
            nearest = baseStations.Where(item => item.DistanceTo(location) == minDistance).LastOrDefault();
            return nearest;
        }
        #endregion

        #region customr&location

        /// <summary>
        /// the func checks what the location of a certain customer
        /// </summary>
        /// <param name="item">the first DO.Customer object</param>
        /// <returns>location object</returns>
        private Location getCustomerLocation(DO.Customer item)
        {
            Location location = new Location();
            location.Latitude = item.Latitude;
            location.Longitude = item.Longitude;
            return location;
        }

        /// <summary>
        /// the func looks for a customer in the list of the customers from
        /// the dal according to id
        /// </summary>
        /// <param name="id">the first int value</param>
        /// <returns>customer object</returns>
        private Customer findCustomer(int id)
        {
            Customer customer = new Customer();
            try
            {
                lock (dal)
                {
                    customer = convertToCustomer(dal.RequestCustomer(id));
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException("customer", e);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch(BO.XMLFileLoadCreateException e) { throw e; }
            return customer;
        }

        /// <summary>
        /// the func checks what the location of a certain sender according to his id
        /// </summary>
        /// <param name="id">the first int value</param>
        /// <returns>location object</returns>
        private Location senderLocation(int id)
        {
            DO.Customer customer = new DO.Customer();
            try
            {
                lock (dal)
                {
                    customer = dal.RequestCustomer(id);
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException("customer", e);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            return new Location { Latitude = customer.Latitude, Longitude = customer.Longitude };
        }
        #endregion

        /// <summary>
        /// the func checks if a drone is responsible for a parcel which has almost 
        /// belonged to it but does not picked up yet.If the func finds this details,
        /// then it initializes the fields of this DroneForList.
        /// </summary>
        /// <param name="d">the first drone object</param>
        /// <param name="droneForList">the second out DroneForListobject</param>
        /// <param name="senderId">the 3th out int value</param>
        /// <param name="parcelId">the 4th out int value</param>
        /// <returns>bool value</returns>
        private bool checkForPickedUp(Drone d, out DroneForList droneForList, out int senderId, out int parcelId)
        {
            bool find = false;
            parcelId = -1;
            senderId = -1;
            droneForList = null;
            foreach (var drone in drones)
            {
                if (drone.Id == d.Id && drone.DeliveryId > 0 && !find)
                {
                    DO.Parcel parcel = new DO.Parcel();
                    try
                    {
                        lock (dal)
                        {
                            parcel = dal.RequestParcel(drone.DeliveryId);
                        }
                    }
                    catch(DO.XMLFileLoadCreateException e)
                    {
                        throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                    }
                    if (parcel.Scheduled != null && parcel.PickedUp == null)
                    {
                        find = true;
                        parcelId = parcel.Id;
                        senderId = parcel.SenderId;
                        droneForList = drone;
                        drones.Remove(drone);
                        return find;
                    }
                }
            }
            return find;
        }

        #region BatteryCalculate

        /// <summary>
        /// the func check if there is enough battery for a certain distance
        /// </summary>
        /// <param name="distance">the first double value</param>
        /// <param name="battery">the second int value</param>
        /// <param name="weight">the 3th int value</param>
        /// <returns>bool value</returns>
        private bool enough(double distance, double battery, int weight)
        {
            double sumBattery = 0.0;
            try
            {
                sumBattery = minBattery(distance, weight);
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            return sumBattery <= battery;
        }

        /// <summary>
        /// The function calculates how much battery the drone is filled
        /// according to its charging time
        /// </summary>
        /// <param name="time">the first int value</param>
        /// <returns>int value</returns>
        private double calculateBattery(int time)
        {
            double sumBattery = (int)(((time / 60) + ((time % 60) / 60)) * chargingRate);
            if (sumBattery > 100)
            {
                return 100;
            }
            return sumBattery;
        }

        /// <summary>
        /// the func calculates how much battery a drone needs for a certain distance
        /// </summary>
        /// <param name="distance">the first double value</param>
        /// <param name="weight">the second int value</param>
        /// <returns>double value</returns>
        private double minBattery(double distance, int weight)
        {
            double sumBattery;
            switch (weight)
            {
                case 0:
                    sumBattery = available * distance;
                    break;
                case 1:
                    sumBattery = lightWeight * distance;
                    break;
                case 2:
                    sumBattery = mediumWeight * distance;
                    break;
                case 3:
                    sumBattery = heavyWeight * distance;
                    break;
                default:
                    throw new DiscrepanciesException("failed to calculate minimum battery which a drone need for a way");
            }
            return sumBattery;
        }
        #endregion

        /// <summary>
        /// the func initializes the list of drones in the BL
        /// </summary>
        private void initDrones()
        {
            IEnumerable<DO.Drone> dalDrones;
            try
            {
                lock (dal)
                {
                    dalDrones = dal.RequestListDrones();
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            drones = (dalDrones.Select(item => new DroneForList { Id = item.Id, Model = item.Model, Weight = (WeightCategories)item.MaxWeight })).ToList();
            foreach (DroneForList item in drones)
            {
                item.Battery = 1;
                item.DeliveryId = 0;
                try
                {
                    item.Location = getLocation(item);
                }
                catch (DiscrepanciesException e)
                {
                    throw e;
                }
                catch (UnextantException e)
                {
                    throw e;
                }
                catch(BO.XMLFileLoadCreateException e) { throw e; }
            }
        }

        /// <summary>
        /// the func checks what the location of a certain drone 
        /// according the instructions
        /// </summary>
        /// <param name="drone">the first DroneForList object</param>
        /// <returns>location object</returns>
        private Location getLocation(DroneForList drone)
        {
            int index;
            IEnumerable<DO.Parcel> dalParcels = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    dalParcels = dal.RequestPartListParcels(item => item.Scheduled != null && item.Delivered == null && drone.Id == item.DroneId);
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            foreach (DO.Parcel item in dalParcels)
            {
                drone.Status = DroneStatuses.TRANSPORT;
                Customer sender = new Customer(), target = new Customer();
                double distance1 = 0;
                int min;
                try
                {
                    findSenderAndTarget(item, out sender, out target);
                    distance1 = drone.DistanceTo(sender) + target.DistanceTo(nearStation(target));
                    double distance2 = sender.DistanceTo(target);
                    min = (int)(minBattery(distance1, 0) + minBattery(distance2, (int)item.Weight + 1)) + 1;
                }
                catch(BO.XMLFileLoadCreateException e) { throw e; }
                catch (DiscrepanciesException e)
                {
                    throw e;
                }
                catch (UnextantException e)
                {
                    throw e;
                }
                drone.Battery = ran.Next(min, 101)+0.0;
                drone.DeliveryId = item.Id;

                // Associated and not collected
                if (item.PickedUp == null)
                {
                    try
                    {
                        return nearStation(drone).Location;
                    }
                    catch (DiscrepanciesException e)
                    {
                        throw e;
                    }
                    catch(BO.XMLFileLoadCreateException e) { throw e; }
                }

                // Associated, collected and not provided
                else
                {
                    try
                    {
                        return senderLocation(item.SenderId);
                    }
                    catch (UnextantException e)
                    {
                        throw e;
                    }
                    catch(BO.XMLFileLoadCreateException e)
                    {
                        throw e;
                    }
                }
            }

            if (drone.Status != DroneStatuses.TRANSPORT)
            {
                List<DroneCharge> charges = new List<DroneCharge>();
                try
                {
                    lock (dal)
                    {
                        charges = dal.RequestDroneCharges();
                    }
                }
                catch (DO.XMLFileLoadCreateException e)
                {
                    throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                }
                if(length==0)
                    drone.Status = (DroneStatuses)ran.Next(0, 2);
                else
                {
                    drone.Status = DroneStatuses.AVAILABLE;
                    DroneCharge d = charges.FirstOrDefault(dr => dr.DroneId == drone.Id);
                    if (d.DroneId != 0) {
                        drone.Status = DroneStatuses.MAINTENANCE;
                        drone.Battery = ran.Next(0, 21);
                        DO.BaseStation b = new DO.BaseStation();
                        try
                        {
                            lock (dal)
                            {
                                b = dal.RequestBaseStation(((DroneCharge)d).StationId);
                            }
                        }
                        catch (DO.UnextantException e)
                        {
                            throw new BO.UnextantException(e.Message, e);
                        }
                        catch (DO.XMLFileLoadCreateException e)
                        {
                            throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                        }
                        return new Location() { Latitude = b.Latitude, Longitude = b.Longitude };
                    }
                }
                if (drone.Status == DroneStatuses.MAINTENANCE && length==0)
                {
                    drone.Battery = ran.Next(0, 21);
                    try
                    {
                        lock (dal)
                        {
                            Location temp = randomStation(out int Id);
                            DO.BaseStation baseStation = dal.RequestBaseStation(Id);
                            while (!haveAvailableChargeSlots(baseStation))
                            {
                                temp = randomStation(out Id);
                                baseStation = dal.RequestBaseStation(Id);
                            }
                            DO.DroneCharge droneCharge = new DO.DroneCharge();
                            droneCharge.DroneId = drone.Id;
                            droneCharge.StationId = baseStation.Id;
                            dal.UpdateCharge(droneCharge);
                            return temp;
                        }
                    }
                    catch (DiscrepanciesException e)
                    {
                        throw e;
                    }
                    catch (DO.ExtantException e)
                    {
                        throw new BO.ExtantException(e.Message, e);
                    }
                    catch(DO.XMLFileLoadCreateException e)
                    {
                        throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                    }
                    catch(BO.XMLFileLoadCreateException e) { throw e; }
                }
                else
                {
                    Location l = new Location();
                    List<Location> locations = new List<Location>();
                    try
                    {
                        lock (dal)
                        {
                            foreach (var parcel in dal.RequestPartListParcels((parcel => parcel.Delivered != null)))
                            {
                                IEnumerable<DO.Customer> dalCustomers = dal.RequestPartListCustomers(c => c.Id == parcel.TargetId);
                                locations = (dalCustomers.Select(customer => new Location()
                                {
                                    Longitude = customer.Longitude,
                                    Latitude = customer.Latitude
                                })).ToList();
                            }
                        }
                    }
                    catch (DO.XMLFileLoadCreateException e)
                    {
                        throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                    }

                    if (locations.Count != 0)
                    {
                        index = ran.Next(locations.Count);
                        l = locations[index];
                    }
                    else
                    {
                        l.Longitude = ran.Next(0, 90);
                        l.Latitude = ran.Next(0, 180);
                    }
                    drone.Location = l;
                    try
                    {
                        double distance = nearStation(drone).DistanceTo(drone);
                        drone.Battery = ran.Next((int)minBattery(distance, 0) + 1, 101);
                    }
                    catch (DiscrepanciesException e)
                    {
                        throw e;
                    }
                    catch (BO.XMLFileLoadCreateException e) { throw e; }
                    return l;
                }
            }
            try
            {
                return randomStation(out int Id);
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch(BO.XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// the func finds the sender and the target of a parcel
        /// </summary>
        /// <param name="parcel">the first DO.Parcel object</param>
        /// <param name="sender">the second Customer object</param>
        /// <param name="target">the 3th Customer object</param>
        private void findSenderAndTarget(DO.Parcel parcel, out Customer sender, out Customer target)
        {
            try
            {
                lock (dal)
                {
                    sender = convertToCustomer(dal.RequestCustomer(parcel.SenderId));
                    target = convertToCustomer(dal.RequestCustomer(parcel.TargetId));
                }
            }
            catch (DO.UnextantException e)
            {
                throw new BO.UnextantException(e.Message, e);
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// the func looks for a parcel to belong its to a drone
        /// </summary>
        /// <param name="optionalParcels">the first List<ParcelForList> object</param>
        /// <param name="d">the second drone object</param>
        /// <returns>int value</returns>
        private int findParcel(List<ParcelForList> optionalParcels, Drone d)
        {
            List<ParcelForList> temp = new List<ParcelForList>();
            List<ParcelForList> temp2 = new List<ParcelForList>();
            int index = 2;
            do
            {
                temp = (optionalParcels.Where(item => item.Priority == (Priorities)index).Select(item => item)).ToList();
                --index;
            } while (index >= 0 && temp.Count == 0);

            index = 2;
            do
            {
                temp2 = (temp.Where(item => item.Weight == (WeightCategories)index).Select(item => item)).ToList();
                --index;
            } while (index >= 0 && temp2.Count == 0);

            List<ParcelInTransfer> parcels = new List<ParcelInTransfer>();
            IEnumerable<DO.Parcel> parcelList = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    parcelList = dal.RequestListParcels();
                }
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            try
            {
                parcels = (from item in parcelList
                           from item2 in temp2
                           where item2.Id == item.Id
                           select convertToParcelInTransfer(item, false)).ToList();
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            double minDistance = parcels[0].DistanceTo(d);
            ParcelInTransfer nearParcel = parcels[0];
            minDistance = parcels.Min(item => item.DistanceTo(d));
            nearParcel = parcels.Where(item => item.DistanceTo(d) == minDistance).LastOrDefault();
            return nearParcel.Id;
        }

        /// <summary>
        /// the func checks what the status of a certain parcel
        /// </summary>
        /// <param name="item">the first DO.Parcel object</param>
        /// <returns>enum value</returns>
        private Enum.DeliveryStatus getDeliveryStatus(DO.Parcel item)
        {
            if (item.Delivered != null)
            {
                return Enum.DeliveryStatus.PROVIDED;
            }
            else
            {
                if (item.PickedUp != null)
                {
                    return DeliveryStatus.COLLECTED;
                }
                else
                {
                    if (item.Scheduled != null)
                    {
                        return DeliveryStatus.BELONGED;
                    }
                    else
                    {
                        if (item.Requested != null)
                        {
                            return DeliveryStatus.CREATED;
                        }
                        else
                        {
                            throw new DiscrepanciesException("failed to give a status to a parcel");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// check if the parcel was scheduled
        /// </summary>
        /// <param name="parcel">first parcel object</param>
        /// <returns>bool</returns>
        bool isScheduled(DO.Parcel parcel)
        {
            return parcel.Scheduled != null;
        }

        internal void refreshDrones(Drone drone)
        {
            DroneForList temp = drones.FirstOrDefault(d => d.Id == drone.Id);
            if(temp == null)
            {
                throw new UnextantException("drone");
            }
            drones.Remove(temp);
            temp.Battery = drone.Battery;
            if(drone.DeliveryByTransfer == null || drone.DeliveryByTransfer.Id ==  0)
            {
                temp.DeliveryId = 0;
            }
            else
            {
                temp.DeliveryId = (int)(drone.DeliveryByTransfer.Id);
            }
            temp.Location = new Location() { Longitude = drone.Location.Longitude, Latitude = drone.Location.Latitude};
            temp.Model = drone.Model;
            temp.Status = drone.Status;
            temp.Weight = drone.Weight;
            drones.Add(temp);
        }
    }
}
