using System;
using System.Collections.Generic;
using DO;

namespace DS
{
    public static class DataSource
    {
        const int DRONE_SIZE = 10;
        const int BASE_STAION_SIZE = 5;
        const int CUSTOMERS_SIZE = 100;
        const int PARCEL_SIZE = 1000;
        internal static List<Drone> DronesList = new List<Drone>();
        internal static List<BaseStation> BaseStationsList = new List<BaseStation>();
        internal static List<Customer> CustomersList = new List<Customer>();
        internal static List<Parcel> ParcelsList = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        public struct Confing
        {
            public static int ParcelId = 0;
            public static double Available { get; set; } = 0.0005;
            public static double LightWeight { get; set; } = 0.001;
            public static double MediumWeight { get; set; } = 0.0015;
            public static double HeavyWeight { get; set; } = 0.002;
            public static int ChargingRate { get; set; } = 10;
        }

        static Random ran = new Random();

        /// <summary>
        /// Initialize  is a method in the DataSource class.
        /// the method intilialize the data base for the program
        /// </summary>
        static internal void Initialize()
        {
            for (int i = 0; i < 2; ++i)
            {
                BaseStation b = new BaseStation();
                b.Id = BaseStationsList.Count;
                b.Name = $"station_{'a' + BaseStationsList.Count}";
                b.Longitude = ran.Next(0, 90);
                b.Latitude = ran.Next(0, 180);
                int chargeSlots = ran.Next(3, 8);
                b.ChargeSlots = chargeSlots;
                BaseStationsList.Add(b);
            }

            for (int i = 0; i < 5; ++i)
            {
                Drone d = new Drone();
                d.Id = DronesList.Count + 1;
                d.Model = $"model_{ran.Next(9000, 9011)}";
                d.MaxWeight = (WeightCategories)ran.Next(0, 3);
                DronesList.Add(d);
            }

            for (int i = 0; i < 10; ++i)
            {
                Customer customer = new Customer();
                customer.Id = CustomersList.Count;
                customer.Name = $"customer{CustomersList.Count}";
                customer.Phone = $"05{ran.Next(10000000, 100000000)}";
                customer.Longitude = ran.Next(0, 90);
                customer.Latitude = ran.Next(0, 180);
                CustomersList.Add(customer);
            }

            for (int i = 0; i < 10; ++i)
            {
                Parcel parcel = new Parcel();
                parcel.SenderId = ran.Next(0, CustomersList.Count);
                parcel.TargetId = ran.Next(0, CustomersList.Count);
                parcel.Weight = (WeightCategories)ran.Next(0, 3);
                parcel.Priority = (Priorities)ran.Next(0, 3);
                parcel.Id = Confing.ParcelId++;
                parcel.Requested = DateTime.Now;
                parcel.DroneId = 0;
                parcel.Scheduled = null;
                parcel.PickedUp = null;
                parcel.Delivered =null;
                ParcelsList.Add(parcel);
            }
        }
    }
}
