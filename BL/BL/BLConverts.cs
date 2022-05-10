using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;

namespace BL
{
    partial class BL
    {

        #region baseStationConverters

        /// <summary>
        /// the func converts from DO.BaseStation to BO.BaseStation
        /// </summary>
        /// <param name="blBaseStation">the first basesatation object</param>
        /// <param name="dalBAseStation">the second DO.BaseSatation object</param>
        /// <returns>BaseStation object</returns>
        private BaseStation convertDalBaseStationToBL(DO.BaseStation dalBAseStation)
        {
            BaseStation blBaseStation = new();
            blBaseStation.Id = dalBAseStation.Id;
            blBaseStation.Name = dalBAseStation.Name;
            blBaseStation.Location = new Location();
            blBaseStation.Location.Latitude = dalBAseStation.Latitude;
            blBaseStation.Location.Longitude = dalBAseStation.Longitude;
            try
            {
                blBaseStation.AvailableChargeSlots = dalBAseStation.ChargeSlots - catchChargeSlots(dalBAseStation);
            }
            catch(BO.XMLFileLoadCreateException e) { throw e; }
            IEnumerable<DO.DroneCharge> droneCharges;
            try
            {
                droneCharges = dal.RequestPartListDroneCharges(droneCharge => droneCharge.StationId == blBaseStation.Id);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            blBaseStation.DronesInCharching = (from item in droneCharges
                                               select new DroneInCharging()
                                               {
                                                   Id = item.DroneId,
                                                   BatteryStatus = drones.Find(drone1 => drone1.Id == item.DroneId).Battery
                                               }
                                             ).ToList<DroneInCharging>();
            return blBaseStation;
        }

        /// <summary>
        /// the func converts from DO.BaseStation to BO.BaseStationForList
        /// </summary>
        /// <param name="item">the first DO.BaseSatation object</param>
        /// <returns>BaseStationForList object</returns>
        private BaseStationForList convertBaseStation(DO.BaseStation item)
        {
            int notAvailable;
            try
            {
                notAvailable = catchChargeSlots(item);
            }
            catch(BO.XMLFileLoadCreateException e) { throw e; }
            BaseStationForList b = new BaseStationForList
            { Id = item.Id, Name = item.Name, AvailableChargeSlots = item.ChargeSlots - notAvailable };
            b.CatchChargeSlots = notAvailable;
            return b;
        }
        #endregion

        #region customerConverters

        /// <summary>
        /// the func converts from DO.Customer to BO.Customer
        /// </summary>
        /// <param name="dlCustomer">the first DO.Customer object</param>
        /// <returns>customer object</returns>
        private Customer convertToCustomer(DO.Customer dlCustomer)
        {
            Customer customer = new Customer();
            customer.Id = dlCustomer.Id;
            customer.Name = dlCustomer.Name;
            customer.PhoneNumber = dlCustomer.Phone;
            customer.Location = new Location();
            customer.Location.Latitude = dlCustomer.Latitude;
            customer.Location.Longitude = dlCustomer.Longitude;
            List<ParcelToCustomer> fromCustomer = new List<ParcelToCustomer>();
            List<ParcelToCustomer> toCustomer = new List<ParcelToCustomer>();
            IEnumerable<DO.Parcel> senders = Enumerable.Empty<DO.Parcel>();
            IEnumerable<DO.Parcel> targets = Enumerable.Empty<DO.Parcel>();
            try
            {
                senders = dal.RequestPartListParcels(parcel => parcel.SenderId == dlCustomer.Id);
                targets = dal.RequestPartListParcels(parcel => parcel.TargetId == dlCustomer.Id);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        
            try
            {
                fromCustomer =(senders.Select(s => convertToParcelToCustomer(s))).ToList();
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
            try
            {
                toCustomer = (targets.Select(item => convertToParcelToCustomer(item))).ToList();

            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (BO.XMLFileLoadCreateException e) { throw e; }
            customer.FromCustomer = fromCustomer;
            customer.ToCustomer = toCustomer;
            return customer;
        }

        /// <summary>
        /// the func converts from DO.Customer to BO.CustomerInParcel
        /// </summary>
        /// <param name="c">the first DO.Customer object</param>
        /// <returns>CustomerInParcel object</returns>
        private CustomerInParcel convertToCustomerInParcel(DO.Customer c)
        {
            CustomerInParcel newCustomer = new CustomerInParcel();
            newCustomer.Id = c.Id;
            newCustomer.Name = c.Name;
            return newCustomer;
        }

        #endregion

        #region droneConverers

        /// <summary>
        /// the func converts from BO.DroneForList to BO.Drone
        /// </summary>
        /// <param name="droneForList">the first DroneForList object</param>
        /// <returns>Drone object</returns>
        private Drone convertToDrone(DroneForList droneForList)
        {
            Drone drone = new Drone();
            drone.Id = droneForList.Id;
            drone.Model = droneForList.Model;
            drone.Weight = droneForList.Weight;
            drone.Battery = droneForList.Battery;
            drone.Status = droneForList.Status;
            drone.Location = new Location() { Longitude = droneForList.Location.Longitude, Latitude = droneForList.Location.Latitude };

            if (droneForList.DeliveryId == 0)
            {
                drone.DeliveryByTransfer = null;
            }
            else
            {
                IEnumerable<DO.Parcel> dalParcels;
                try
                {
                    lock (dal)
                    {
                        dalParcels = dal.RequestPartListParcels(parcel => parcel.Id == droneForList.DeliveryId);
                    }
                }
                catch(DO.XMLFileLoadCreateException e)
                {
                    throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
                }
                foreach (var item in dalParcels)
                {
                    bool status = drone.Status == (DroneStatuses)2 ? true : false;
                    try
                    {
                        lock (dal)
                        {
                            drone.DeliveryByTransfer = convertToParcelInTransfer(item, status);
                        }
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
                    return drone;
                }
            }

            return drone;
        }
        #endregion

        #region ParcelConverters

        /// <summary>
        /// the func converts from DO.Parcel to BO.Parcel
        /// </summary>
        /// <param name="dalParcel">the first DO.Parcel object</param>
        /// <returns>parcek object</returns>
        private Parcel convertToParcel(DO.Parcel dalParcel)
        {
            Parcel parcel = new Parcel();
            parcel.Id = dalParcel.Id;
            try
            {
                lock (dal)
                {
                    parcel.Sender = convertToCustomerInParcel(dal.RequestCustomer(dalParcel.SenderId));
                    parcel.Target = convertToCustomerInParcel(dal.RequestCustomer(dalParcel.TargetId));
                } 
            }
            catch (DO.UnextantException e)
            {
                throw new BO.UnextantException(e.Message, e);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch(BO.XMLFileLoadCreateException e) { throw e; }
            parcel.Scheduled = dalParcel.Scheduled;

            if (parcel.Scheduled != null&&dalParcel.Delivered==null)
            {
                DroneForList d = new DroneForList();
                d = drones.Find(drone => drone.DeliveryId == dalParcel.Id);
                if(d == null)
                {
                    throw new DiscrepanciesException("failed to convert to parcel");
                }
                if(d != null)
                {
                    parcel.Drone1 = new DroneInParcel()
                    {
                        Id = d.Id,
                        BatteryStatus = d.Battery,
                        Current = d.Location
                    };
                }
            }
            else
            {
                parcel.Drone1 = null;
            }

            parcel.Weight = (WeightCategories)dalParcel.Weight;
            parcel.Priority = (Priorities)dalParcel.Priority;
            parcel.Created = dalParcel.Requested;
            parcel.PickedUp = dalParcel.PickedUp;
            parcel.Delivered = dalParcel.Delivered;
            return parcel;
        }

        /// <summary>
        /// the func converts from DO.Parcel to BO.ParcelForList
        /// </summary>
        /// <param name="item">the first DO.Parcel object</param>
        /// <returns>ParcelForList object</returns>
        private ParcelForList convertParcel(DO.Parcel item)
        {
            ParcelForList p = new ParcelForList { Id = item.Id, Weight = (WeightCategories)item.Weight, Priority = (Priorities)item.Priority, SenderName = "", TargetName = "" };
            try
            {
                lock (dal)
                {
                    p.SenderName = dal.RequestCustomer(item.SenderId).Name;
                    p.Status = (DeliveryStatus)getDeliveryStatus(item);
                    p.TargetName = dal.RequestCustomer(item.TargetId).Name;
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
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch(BO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return p;
        }

        /// <summary>
        /// the func converts from DO.Parcel to BO.ParcelToCustomer
        /// </summary>
        /// <param name="p">the first DO.Parcel object</param>
        /// <returns>ParcelToCustomer object</returns>
        private ParcelToCustomer convertToParcelToCustomer(DO.Parcel p)
        {
            ParcelToCustomer newParcel = new ParcelToCustomer();
            newParcel.Id = p.Id;
            newParcel.Weight = (WeightCategories)p.Weight;
            newParcel.Priority = (Priorities)p.Priority;
            CustomerInParcel target = null;
            CustomerInParcel sender = null;
            try
            {
                lock (dal)
                {
                    newParcel.Status = (DeliveryStatus)getDeliveryStatus(p);
                    target = convertToCustomerInParcel(dal.RequestCustomer(p.TargetId));
                    sender = convertToCustomerInParcel(dal.RequestCustomer(p.SenderId)); 
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (DO.UnextantException e)
            {
                throw new BO.UnextantException(e.Message, e);
            }
            if (p.Delivered == null)
            {
                newParcel.Partner = target;
            }
            else
            {
                newParcel.Partner = sender;
            }
            return newParcel;
        }

        /// <summary>
        /// the func converts from DO.Parcel to BO.ParcelInTransfer
        /// </summary>
        /// <param name="parcel">the first DO.Parcel object</param>
        /// <param name="status">the Second bool value</param>
        /// <returns>ParcelInTransfer object</returns>
        private ParcelInTransfer convertToParcelInTransfer(DO.Parcel parcel, bool status)
        {
            ParcelInTransfer parcelInTransfer = new ParcelInTransfer();
            parcelInTransfer.Id = parcel.Id;
            parcelInTransfer.status = status;
            parcelInTransfer.Priority = (Priorities)parcel.Priority;
            parcelInTransfer.Weight = (WeightCategories)parcel.Weight;
            parcelInTransfer.Target = new CustomerInParcel();
            parcelInTransfer.Sender = new CustomerInParcel();
            parcelInTransfer.Target.Id = parcel.TargetId;
            parcelInTransfer.Sender.Id = parcel.SenderId;
            DO.Customer targets = new DO.Customer();
            DO.Customer senders = new DO.Customer();
            try
            {
                lock (dal)
                {
                    targets = (dal.RequestPartListCustomers(customer => customer.Id == parcelInTransfer.Target.Id)).FirstOrDefault();
                    senders = (dal.RequestPartListCustomers(customer => customer.Id == parcelInTransfer.Sender.Id)).FirstOrDefault();
                }
    
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            parcelInTransfer.Target.Name = targets.Name;
            parcelInTransfer.Sender.Name = senders.Name;
            DO.Customer target = new DO.Customer();
            DO.Customer sender = new DO.Customer();
            try
            {
                lock (dal)
                {
                    target = dal.RequestCustomer(parcel.TargetId);
                    sender = dal.RequestCustomer(parcel.SenderId);
                    parcelInTransfer.Destination = getCustomerLocation(target);
                    parcelInTransfer.Location = getCustomerLocation(sender);
                    parcelInTransfer.TransportDistance = convertToCustomer(sender).DistanceTo(convertToCustomer(target));
                }
              
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (DO.UnextantException e)
            {
                throw new BO.UnextantException(e.Message, e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return parcelInTransfer;
        }
        #endregion

    }
}