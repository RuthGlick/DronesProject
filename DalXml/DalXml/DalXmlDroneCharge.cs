using System.Runtime.CompilerServices;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    internal partial class DalXml
    {
        /// <summary>
        /// the method returns List of the Drone Charge
        /// </summary>
        /// <returns>List of DroneCharge objects</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<DroneCharge> RequestDroneCharges()
        {
            List<DroneCharge> list;
            try
            {
              return list = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// the method Create a new Charge Slot 
        /// </summary>
        /// <param name="droneCharge">DroneCharge object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateChargeSlot(DroneCharge droneCharge)
        {
            List<DroneCharge> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            if (list.Contains(droneCharge))
            {
                throw new ExtantException("drone charge");
            }
            list.Add(droneCharge);
            list.Sort();
            try
            {
                XMLTools.SaveListToXmlSerializer<DroneCharge>(list, droneChargesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of drone charge</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> RequestPartListDroneCharges(Predicate<DroneCharge> predicate)
        {
            List<DroneCharge> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(droneCharge => predicate(droneCharge));
        }

        /// <summary>
        /// UpdateRelease is a method in the DalXml class.
        /// the method removes a drone from charging at a base station
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateRelease(DroneCharge d)
        {
            bool isDroneInCharge = false;
            List<DroneCharge> list= new List<DroneCharge>();
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            DroneCharge droneCharge = list.FirstOrDefault(dc => dc.DroneId == d.DroneId);
            if (droneCharge.DroneId != 0)
            {
                isDroneInCharge = true;
                list.Remove((DroneCharge)droneCharge);
            }
            if (!isDroneInCharge)
            {
                throw new UnextantException("drone charge");
            }
            try
            {
                XMLTools.SaveListToXmlSerializer<DroneCharge>(list, droneChargesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }
    }
}
