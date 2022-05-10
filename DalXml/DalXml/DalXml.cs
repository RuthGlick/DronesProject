using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DalApi;
using DO;

namespace DAL
{
    internal sealed partial class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        internal static IDal Instance { get => instance; }

        private readonly string baseStationsPath = "BaseStations.xml";
        private readonly string dronesPath = "Drones.xml";
        private readonly string parcelsPath = "Parcels.xml";
        private readonly string customersPath = "Customers.xml";
        private readonly string droneChargesPath = "DroneCharges.xml";
        private readonly string data = "Config.xml";


        private DalXml()
        {
        }

    }
}
