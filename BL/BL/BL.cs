using DalApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Enum;
using BlApi;

namespace BL
{
    sealed partial class BL : IBL
    {
        static readonly IBL instance = new BL();
        internal static IBL Instance { get => instance; }

        private List<DroneForList> drones = new List<DroneForList>(); 
        internal IDal dal;
        private double available;
        private double lightWeight;
        private double mediumWeight;
        private double heavyWeight;
        private readonly int length;
        private double chargingRate;

        /// <summary>
        /// BL constructor initialize the electricity consumption by the drones and
        /// their rate of loading. It also initializes the list of drones of BL.
        /// </summary>
        BL()
        {
            try
            {
                   dal = DalFactory.GetDal();
            } 
            catch (DO.DalConfigException e)
            {
                throw new BLConfigException(e.ToString(), e);
            }
            double[] arr;
            arr = dal.RequestElectricity();
            available = arr[0];
            lightWeight = arr[1];
            mediumWeight = arr[2];
            heavyWeight = arr[3];
            chargingRate = arr[4];
            drones = new List<DroneForList>();
            try
            {
                lock (dal)
                {
                     length = dal.RequestDroneCharges().Count;
                     initDrones();
                }
               
            }
            catch(DiscrepanciesException e)
            {
                throw e;
            }
            catch(UnextantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch(DO.XMLFileLoadCreateException e)
            {
                throw new BO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            catch (DO.DalConfigException e) 
            {
                throw new BLConfigException(e.Message);
            }
        }

        public void StartDroneSimulator(int id, Action update, Func<bool> checkStop) =>
            new Simulator(this, id, update, checkStop);
    }
}
        

 