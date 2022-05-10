using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;
using DalApi;
using BlApi;
using System.Collections.Generic;

namespace BL
{
    class Simulator
    {
        enum Maintenance { Starting, Going, Charging, Waiting }
        private const double VELOCITY = 1.0;
        private const int DELAY = 20;
        private const double STEP = VELOCITY / (DELAY / 1000.0);
        BL bl;
        BO.Drone drone;
        Maintenance maintenance;
        double distance;
        IDal dal;
        Customer customer;
        BaseStation bs = new BaseStation();
        DO.Parcel parcel = new();
        BaseStation nearest = new BaseStation();

        /// <summary>
        /// simulator
        /// </summary>
        /// <param name="bl">first BL obj</param>
        /// <param name="droneId">second int value</param>
        /// <param name="updateDrone">3th Actiontype</param>
        /// <param name="checkStop">4th Func<bool> type</param>
        public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> checkStop)
        {
            this.bl = bl;
            lock (bl)
            {
                dal = bl.dal;
            }
            drone = new Drone() { Id = droneId };
            try
            {
                lock (bl)
                {
                    drone = bl.RequestDrone(drone);
                }
            }
            catch (UnextantException e) { throw e; }
            catch (XMLFileLoadCreateException e) { throw e; }
            catch (DiscrepanciesException e) { throw e; }
            maintenance = Maintenance.Charging;
            do
            {
                if (!sleepDelayTime()) { break; }
                switch (drone.Status)
                {
                    case Enum.DroneStatuses.AVAILABLE:
                        try
                        {
                            caseAvailable();
                        }
                        catch(XMLFileLoadCreateException e) { throw e; }
                        catch(DiscrepanciesException e) { throw e; }
                        catch(UnextantException e) { throw e; }
                        catch(ExtantException e) { throw e; }
                        break;
                    case Enum.DroneStatuses.MAINTENANCE:
                        try
                        {
                            caseMaintenance();
                        }
                        catch (XMLFileLoadCreateException e) { throw e; }
                        catch (DiscrepanciesException e) { throw e; }
                        catch (UnextantException e) { throw e; }
                        catch (ExtantException e) { throw e; }
                        break;
                    case Enum.DroneStatuses.TRANSPORT:
                        try
                        {
                            caseTransport();
                        }
                        catch (XMLFileLoadCreateException e) { throw e; }
                        catch (DiscrepanciesException e) { throw e; }
                        catch (UnextantException e) { throw e; }
                        break;
                    default:
                        break;
                }
                try
                {
                    updateDrone();
                }
                catch { }
            } while (!checkStop());
        }

        /// <summary>
        /// The function handles the drone when transferring delivery and reports pickup and package delivery
        /// </summary>
        private void caseTransport()
        {
            try
            {
                lock (dal)
                {
                    parcel = dal.RequestParcel(drone.DeliveryByTransfer.Id);
                }
            }
            catch (DO.XMLFileLoadCreateException e) 
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch(DO.UnextantException e) 
            {
                throw new BO.UnextantException(e.Message);
            }
            int customerId = parcel.PickedUp != null ? parcel.TargetId : parcel.SenderId;
            customer = new Customer() { Id = customerId };
            try
            {
                lock (bl)
                {
                    customer = bl.RequestCustomer(customer);
                }
            }
            catch (DiscrepanciesException e) { throw e; }
            catch (UnextantException e) { throw e; }
            catch(XMLFileLoadCreateException e) { throw e; }
            distance = customer.DistanceTo(drone);
            if(distance < 0.01 || drone.Battery == 0)
            {
                drone.Location = new Location() { Longitude = customer.Location.Longitude, Latitude = customer.Location.Latitude };
                if(parcel.PickedUp != null)
                {
                    try
                    {
                        lock (bl)
                        {
                            bl.UpdateSupply(drone);
                        }
                    }
                    catch (UnextantException e) { throw e; }
                    catch(DiscrepanciesException e) { throw e; }
                    catch(XMLFileLoadCreateException e) { throw e; }
                    drone.Status = Enum.DroneStatuses.AVAILABLE;
                    drone.DeliveryByTransfer.Id = 0;
                }
                else
                {
                    try
                    {
                        lock (bl)
                        {
                            bl.UpdatePickedUp(drone);
                            customer = bl.RequestCustomer(new Customer() { Id = parcel.TargetId });
                        }
                    }
                    catch(UnextantException e) { throw e; }
                    catch(DiscrepanciesException e) { throw e; }
                    catch(XMLFileLoadCreateException e) { throw e; }
                }
            }
            else
            {
                int index;
                switch (parcel.Weight)
                {
                    case DO.WeightCategories.LIGHT:
                        index = 1;
                        break;
                    case DO.WeightCategories.INTERMEDIATE:
                        index = 2;
                        break;
                    case DO.WeightCategories.HEAVY:
                        index = 3;
                        break;
                    default:
                        index = 1;
                        break;
                }
                double delta = distance < STEP ? distance : STEP;
                double proportion = delta / distance;
                try
                {
                    lock (dal)
                    {
                        drone.Battery = Math.Max(0, drone.Battery - delta * dal.RequestElectricity()[index]);
                    }
                }
                catch(DO.XMLFileLoadCreateException e)
                {
                    throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                }
                double latitude = drone.Location.Latitude + (customer.Location.Latitude - drone.Location.Latitude) * proportion;
                double longitude = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * proportion;
                drone.Location = new Location() { Longitude = longitude, Latitude = latitude };
            }
            try
            {
                lock (bl)
                {
                    bl.refreshDrones(drone);
                }
            }
            catch(UnextantException e) { throw e; }
        }

        /// <summary>
        /// The function handles the drone while the charge and reports the charge and release
        /// </summary>
        private void caseMaintenance()
        {
            switch (maintenance)
            {
                case Maintenance.Waiting:
                    try
                    {
                        lock(bl) lock (dal)
                        {
                            int inCharging = dal.RequestPartListDroneCharges(droneCharge => droneCharge.StationId == nearest.Id).Count();
                            if (nearest.DronesInCharching.Count > inCharging)
                            {
                                bl.UpdateCharge(drone);
                                maintenance = Maintenance.Starting;
                            }
                        }
                    }
                    catch(DO.XMLFileLoadCreateException e)
                    {
                        throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                    }
                    catch(UnextantException e) { throw e; }
                    catch(DiscrepanciesException e) { throw e; }
                    catch(XMLFileLoadCreateException e) { throw e; }
                    catch(ExtantException e) { throw e; }
                    break;
                case Maintenance.Starting:
                    try
                    {
                        lock(bl) lock (dal)
                        {
                            int bsId = dal.RequestPartListDroneCharges(dr => dr.DroneId == drone.Id).FirstOrDefault().StationId;
                            if (bsId == 0) { throw new UnextantException("drone charge"); }
                            bs = bl.RequestBaseStation(new BaseStation() { Id = bsId });
                            distance = bs.DistanceTo(drone);
                            maintenance = Maintenance.Going;
                        }
                    }
                    catch(DO.XMLFileLoadCreateException e)
                    {
                        throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                    }
                    catch(UnextantException e) { throw e; }
                    catch(XMLFileLoadCreateException e) { throw e; }
                    break;
                case Maintenance.Going:
                    if(distance<0.01)
                    {
                        drone.Location = new Location() { Longitude = bs.Location.Longitude, Latitude = bs.Location.Latitude};
                        maintenance = Maintenance.Charging;
                    }
                    else
                    {
                        double delta = distance < STEP ? distance : STEP;
                        distance -= delta;
                        try
                        {
                            lock (dal)
                            {
                                drone.Battery = Math.Max(0, drone.Battery - delta * dal.RequestElectricity()[0]);
                            }
                        }
                        catch(DO.XMLFileLoadCreateException e)
                        {
                            throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                        }
                    }
                    break;
                case Maintenance.Charging:
                    if(drone.Battery==100)
                    {
                        try
                        {
                            lock (bl)
                            {
                                bl.UpdateRelease(new DroneInCharging() { Id = drone.Id, BatteryStatus = drone.Battery }, 0);
                            }
                        }
                        catch (UnextantException e) { throw e; }
                        catch(DiscrepanciesException e) { throw e; }
                        catch(XMLFileLoadCreateException e) { throw e; }
                        drone.Status = Enum.DroneStatuses.AVAILABLE;
                    }
                    else
                    {
                        double plus;
                        try
                        {
                            lock (dal)
                            {
                                plus = dal.RequestElectricity()[4] * (DELAY / 1000.0);
                            }
                        }
                        catch(DO.XMLFileLoadCreateException e)
                        {
                            throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                        }
                        double battery = (drone.Battery + plus);
                        drone.Battery = Math.Min(100, battery);
                    }
                    break;
                default:
                    break;
            }
            try
            {
                lock (bl)
                {
                    bl.refreshDrones(drone);
                }
            }
            catch(UnextantException e) { throw e; }
        }

        /// <summary>
        /// The function handles an Available drone and reports the charge and association by case
        /// </summary>
        private void caseAvailable()
        {
            try
            {
                drone.DeliveryByTransfer = null;
                lock (bl)
                {
                    bl.refreshDrones(drone);
                }
            }
            catch (UnextantException e) { throw e; }
            try
            {
                lock (bl)
                {
                    bl.UpdateScheduled(drone);
                }
            }
            catch (UnextantException)
            {
                drone.DeliveryByTransfer = null;
            }
            catch(XMLFileLoadCreateException e) { throw e; }
            catch(DiscrepanciesException e) { throw e; }
            try
            {
                lock (bl)
                {
                    drone = bl.RequestDrone(drone);
                }
            }
            catch(UnextantException e) { throw e; }
            catch(XMLFileLoadCreateException e) { throw e; }
            catch(DiscrepanciesException e) { throw e; }
            switch (drone.DeliveryByTransfer,drone.Battery)
            {
                case (null, 100):
                    break;
                case (null, _):
                    try
                    {
                        lock (bl)
                        {
                            bl.UpdateCharge(drone);
                            drone.Status = Enum.DroneStatuses.MAINTENANCE;
                            maintenance = Maintenance.Starting;
                        }
                    }
                    catch (UnextantException e)
                    {
                        if (e.Message == "base station")
                        lock(bl)
                        {
                            nearest = bl.nearStation(drone);
                            drone.Status = Enum.DroneStatuses.MAINTENANCE;
                            maintenance = Maintenance.Waiting;
                        }
                        else { throw e; }
                    }
                    catch(DiscrepanciesException e) { throw e; }
                    catch(XMLFileLoadCreateException e) { throw e; }
                    catch(ExtantException e) { throw e; }
                    break;
                case (_, _):
                    drone.Status = Enum.DroneStatuses.TRANSPORT;
                    break;
            }
            try
            {
                lock (bl)
                {
                    bl.refreshDrones(drone);
                }
            }
            catch(UnextantException e) { throw e; }
        }

        /// <summary>
        /// the sleepDelayTime func couse the Thread. to Sleep "DELAY" mili-sec
        /// </summary>
        /// <returns>static bool</returns>
        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}