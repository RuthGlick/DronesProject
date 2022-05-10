using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        void CreateBaseStation(BaseStation baseStation);
        void CreateDrone(Drone drone);
        void CreateCustomer(Customer customer);
        void CreateParcel(Parcel parcel);
        void CreateChargeSlot(DroneCharge chargeSlot);
        void UpdateScheduled(Parcel p);
        void UpdateSupply(Parcel p);
        void UpdatePickedUp(Parcel p);
        void UpdateCharge(DroneCharge dc);
        void UpdateRelease(DroneCharge d);
        void UpdateDrone(Drone drone, string newModel);
        void UpdateBaseStation(BaseStation b);
        void UpdateCustomer(Customer c);
        BaseStation RequestBaseStation(int Id);
        Drone RequestDrone(int Id);
        Customer RequestCustomer(int Id);
        Parcel RequestParcel(int Id);
        IEnumerable<BaseStation> RequestListBaseStations();
        IEnumerable<Drone> RequestListDrones();
        IEnumerable<Customer> RequestListCustomers();
        IEnumerable<Parcel> RequestListParcels();
        IEnumerable<Parcel> RequestPartListParcels(Predicate<Parcel> predicate);
        IEnumerable<BaseStation> RequestPartListBaseStations(Predicate<BaseStation> predicate);
        IEnumerable<Drone> RequestPartListDrones(Predicate<Drone> predicate);
        IEnumerable<Customer> RequestPartListCustomers(Predicate<Customer> predicate);
        IEnumerable<DroneCharge> RequestPartListDroneCharges(Predicate<DroneCharge> predicate);
        double[] RequestElectricity();
        List<DroneCharge> RequestDroneCharges();
        void DeleteBaseStation(int id);
        void DeleteParcel(int id);
        void DeleteCustomer(int id);
        void DeleteDrone(int id);
    }
}
