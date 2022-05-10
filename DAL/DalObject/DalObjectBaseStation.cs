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
        /// <summary>
        /// AddBaseStation is a method in the DalObject class.
        /// the method adds a new base station
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateBaseStation(BaseStation baseStation)
        {
            BaseStation exist = BaseStationsList.FirstOrDefault(b => b.Id == baseStation.Id && b.IsDeleted == false);
            if(exist.Id != 0)
            {
                throw new ExtantException("base station");
            }
            baseStation.IsDeleted = false;
            BaseStationsList.Add(baseStation);
        }

        /// <summary>
        /// DisplayBaseStation is a method in the DalObject class.
        /// the method allows base station view
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation RequestBaseStation(int id)
        {
            BaseStation bs = BaseStationsList.FirstOrDefault(b => b.Id == id && b.IsDeleted == false);
            if (bs.Id == 0)
            {
                throw new UnextantException("base station");
            }
            return (BaseStation)bs;
        }

        /// <summary>
        /// ViewListBaseStations is a method in the DalObject class.
        /// the method displays a List of base stations
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> RequestListBaseStations()
        {
            return BaseStationsList.Where(baseStation => baseStation.IsDeleted == false);
        }

        /// <summary>
        /// the func updates name or/and sum of charge slots of a Base-Station
        /// </summary>
        /// <param name="b">the first BaseStation object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation b)
        {
            BaseStation temp = new BaseStation();
            try
            {
                temp = RequestBaseStation(b.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            BaseStationsList.Remove(temp);
            if (b.Name != "")
            {
                temp.Name = b.Name;
            }
            if (b.ChargeSlots > -1)
            {
                temp.ChargeSlots = b.ChargeSlots;
            }
            BaseStationsList.Add(temp);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of basestation</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> RequestPartListBaseStations(Predicate<BaseStation> predicate)
        {
            return BaseStationsList.Where(baseStation => predicate(baseStation) && baseStation.IsDeleted == false);
        }

        /// <summary>
        /// delete base station - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBaseStation(int id)
        {
            BaseStation temp;
            try
            {
                temp = RequestBaseStation(id);
            }
            catch(UnextantException e)
            {
                throw e;
            }
            BaseStationsList.Remove(temp);
            temp.IsDeleted = true;
            BaseStationsList.Add(temp);
        }
    }
}
