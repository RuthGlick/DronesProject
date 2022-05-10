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
        /// AddDrone is a method in the DalXml class.
        /// the method adds a new drone
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateDrone(Drone drone)
        {
            drone.IsDeleted = false;
            List<Drone> dronesXml;
            try
            {
                dronesXml = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            Drone exist;
            exist = dronesXml.FirstOrDefault(d => d.Id == drone.Id && d.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("drone");
            }
            dronesXml.Add(drone);
            try
            {
                XMLTools.SaveListToXmlSerializer<Drone>(dronesXml, dronesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        #region requestMethods

        /// <summary>
        /// the func returns drone by id of drone
        /// </summary>
        /// <param name="id">int value</param>
        /// <returns>drone object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone RequestDrone(int id)
        {
            List<Drone> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            Drone drone = list.FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (drone.Id == 0)
            {
                throw new UnextantException("drone");
            }
            return (Drone)drone;
        }

        /// <summary>
        /// RequestListDrones is a method in the DalXml class.
        /// the method displays the List of drones
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> RequestListDrones()
        {
            List<Drone> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(item => item.IsDeleted == false);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of Drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> RequestPartListDrones(Predicate<Drone> predicate)
        {
            List<Drone> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(drone => predicate(drone) && drone.IsDeleted == false);
        }

        #endregion

        /// <summary>
        /// update drone object - its model
        /// </summary>
        /// <param name="drone">the first Drone value</param>
        /// <param name="newModel">the second string value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone, string newModel)
        {
            Drone temp = new Drone();
            try
            {
                temp = RequestDrone(drone.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch(XMLFileLoadCreateException e) { throw e; }
            List<Drone> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            temp.Model = newModel;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer(list, customersPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// delete Drone - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            Drone temp;
            List<Drone> list;
            try
            {
                temp = RequestDrone(id);
                list = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            catch (UnextantException e) { throw e; }
            list.Remove(temp);
            temp.IsDeleted = true;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer<Drone>(list, dronesPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// UpdateCharge is a method in the DalXml class.
        /// the method updates sending a skimmer for charging at a base station
        /// </summary>
        /// <param name="dc">DroneCharge onject</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCharge(DroneCharge dc)
        {
            try
            {
                CreateChargeSlot(dc);
            }
            catch (ExtantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// the method returns an array of five values according to the data from a xml file - Config, 
        /// in the following order: free, light, medium, heavy and loading rate
        /// </summary>
        /// <returns>array of double values</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] RequestElectricity()
        {
            double[] arr = new double[5];
            XElement config;
            try
            {
                config = XMLTools.LoadListFromXmlElement(@"Config.xml");
            }
            catch(XMLFileLoadCreateException e) { throw e; }
            arr[0] = Convert.ToDouble(config.Element("Available").Value);
            arr[1] = Convert.ToDouble(config.Element("LightWeight").Value);
            arr[2] = Convert.ToDouble(config.Element("MediumWeight").Value);
            arr[3] = Convert.ToDouble(config.Element("HeavyWeight").Value);
            arr[4] = Convert.ToDouble(config.Element("ChargingRate").Value);
            return arr;
        }
    }
}
