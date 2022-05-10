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
        /// the func Create a Base-Station
        /// </summary>
        /// <param name="baseStation">the first basesatation object,new params in BaseStation object for creating </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateBaseStation(BaseStation baseStation)
        {
            // checking if the number of available chargeSlots is valid 
            if (baseStation.AvailableChargeSlots <= 0)
            {
                throw new IncorrectInputException("num of charge slots");
            }
            DO.BaseStation dalBAseStation = new DO.BaseStation();
            dalBAseStation.Id = baseStation.Id;
            dalBAseStation.Name = baseStation.Name;
            dalBAseStation.Longitude = baseStation.Location.Longitude;
            dalBAseStation.Latitude = baseStation.Location.Latitude;
            dalBAseStation.ChargeSlots = baseStation.AvailableChargeSlots + catchChargeSlots(dalBAseStation);
            try 
            {
                lock (dal)
                {
                     dal.CreateBaseStation(dalBAseStation);
                }
               
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("base station", e);
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        /// <summary>
        /// the func Update name or/and sum of charge slots of a Base-Station
        /// </summary>
        /// <param name="b">the first basesatation object,new params in BaseStation object for updating</param>
        /// <param name="chargeSlots">the second int value,new number for charge slots in base station for updating</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation b, int chargeSlots)
        {
            DO.BaseStation newBaseStation = new DO.BaseStation
            {
                Id = b.Id,
                Name = b.Name,
                ChargeSlots = chargeSlots
            };
            try
            {
                lock (dal)
                {
                    dal.UpdateBaseStation(newBaseStation);
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

        #region requestMethods

        /// <summary>
        /// the func requests a base station from the dal
        /// </summary>
        /// <param name="blBaseStation">the first basesatation object</param>
        /// <returns>BaseStation object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation RequestBaseStation(BaseStation blBaseStation)
        {
            DO.BaseStation station = new DO.BaseStation();
            try
            {
                lock (dal)
                {
                   station = dal.RequestBaseStation(blBaseStation.Id);
                   blBaseStation = convertDalBaseStationToBL(station);
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
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            return blBaseStation;
        }

        /// <summary>
        /// the func request the list of the base stations from the dal
        /// </summary>
        /// <returns>IEnumerable of BaseStationForList objects</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationForList> RequestListBaseStations()
        {
            List<BaseStationForList> baseStations = new List<BaseStationForList>();
            IEnumerable<DO.BaseStation> temp;
            try
            {
                lock (dal)
                {
                    temp = dal.RequestListBaseStations();
                    baseStations = (temp.Select(item => convertBaseStation(item))).ToList<BaseStationForList>();
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            return baseStations.OrderBy(b => b.Id);
        }

        /// <summary>
        /// the func requests from the dal a list of base stations which they have available
        /// charge slots
        /// </summary>
        /// <returns> IEnumerable of BaseStationForList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationForList> RequestListAvailableChargeSlots()
        {
            List<BaseStationForList> availableChargeSlots = new List<BaseStationForList>();
            try
            {
                lock (dal)
                {
                    IEnumerable<DO.BaseStation> list = dal.RequestPartListBaseStations(haveAvailableChargeSlots);
                    availableChargeSlots = (list.Select(item => convertBaseStation(item))).ToList();
                }
               
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }

            return availableChargeSlots.OrderBy(b => b.Id);
        }
        #endregion

        /// <summary>
        /// delete base station if the it can be deleted
        /// </summary>
        /// <param name="id">first int value</param>
        /// <returns>bool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteBaseStation(int id)
        {
            BaseStation boBs = new BaseStation() { Id = id};
            try
            {
                boBs = RequestBaseStation(boBs);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }

            if (boBs.DronesInCharching.Count > 0)
            {
                return false;
            }
            try
            {
                lock (dal)
                {
                  dal.DeleteBaseStation(id);
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
            return true;
        }
    }
}
