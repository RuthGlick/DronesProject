using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBL
    {
        void CreateBaseStation(BaseStation b);
        void CreateDrone(Drone blDrone, int IdStation);
        void CreateCustomer(Customer blCustomer);
        void CreateParcel(Parcel blParcel);
        void UpdateDrone(Drone newBlDrone);
        void UpdateBaseStation(BaseStation b, int chargeSlots);
        void UpdateCustomer(Customer c);
        void UpdateRelease(DroneInCharging d, int time);
        void UpdateCharge(Drone d);
        void UpdateScheduled(Drone d);
        void UpdateSupply(Drone d);
        void UpdatePickedUp(Drone d);
        IEnumerable<BaseStationForList> RequestListBaseStations();
        IEnumerable<DroneForList> RequestListDrones();
        IEnumerable<CustomerForList> RequestListCustomers();
        IEnumerable<ParcelForList> RequestListParcels();
        IEnumerable<ParcelForList> RequestListPendingParcels();
        IEnumerable<BaseStationForList> RequestListAvailableChargeSlots();
        IEnumerable<ParcelForList> RequestPartListParcels(Predicate<ParcelForList> predicate);
        IEnumerable<DroneForList> RequestPartListDrones(Predicate<DroneForList> predicate);
        BaseStation RequestBaseStation(BaseStation blBaseStation);
        Drone RequestDrone(Drone blDrone);
        Customer RequestCustomer(Customer blCustomer);
        Parcel RequestParcel(Parcel blParcel);
        bool DeleteBaseStation(int id);
        bool DeleteParcel(int id);
        bool DeleteCustomer(int id);
        bool DeleteDrone(int id);
        void StartDroneSimulator(int id, Action update, Func<bool> checkStop);
    }
}
