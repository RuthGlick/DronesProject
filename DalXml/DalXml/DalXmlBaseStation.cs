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
        /// CreateBaseStation is a method in the DalXml class.
        /// the method adds a new base station
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateBaseStation(BaseStation baseStation)
        {
            XElement rootElem;
            try
            {
                rootElem = XMLTools.LoadListFromXmlElement(baseStationsPath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            if(rootElem.HasElements == true)
            {
                var exist = (from b in rootElem.Elements()
                            where Convert.ToInt32(b.Element("Id").Value) == baseStation.Id && b.Element("IsDeleted").Value == "false"
                            select b).FirstOrDefault();
                if (exist != null)
                {
                    throw new ExtantException("base station");
                }
            }
            rootElem.Add(xmlBaseStation(baseStation));
            try
            {
                XMLTools.SaveListToXmlElement(rootElem, baseStationsPath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
        }

        #region requestMethods

        /// <summary>
        /// RequestBaseStation is a method in the DalXml class.
        /// the method allows base station view
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation RequestBaseStation(int id)
        {
            try
            {
                XElement baseStationsXml = XMLTools.LoadListFromXmlElement(baseStationsPath);
                XElement baseStation = RequestXmlBaseStation(baseStationsXml, id);
                return (new BaseStation()
                {
                    Id = Convert.ToInt32(baseStation.Element("Id").Value),
                    Name = baseStation.Element("Name").Value,
                    ChargeSlots = Convert.ToInt32(baseStation.Element("ChargeSlots").Value),
                    Longitude = Convert.ToDouble(baseStation.Element("Longitude").Value),
                    Latitude = Convert.ToDouble(baseStation.Element("Latitude").Value),
                    IsDeleted = Convert.ToBoolean(baseStation.Element("IsDeleted").Value)
                });
            }
            catch (UnextantException e) { throw e; }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// ViewListBaseStations is a method in the DalXml class.
        /// the method displays a List of base stations
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> RequestListBaseStations()
        {
            XElement baseStationsXML;
            try
            {
                baseStationsXML = XMLTools.LoadListFromXmlElement(baseStationsPath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            IEnumerable<BaseStation> baseStations = Enumerable.Empty<BaseStation>();
            if (baseStationsXML.HasElements == false)
            {
                return baseStations;
            }
            baseStations = (from b in baseStationsXML.Elements()
                            where b.Element("IsDeleted").Value == "false"
                            select new BaseStation()
                            {
                                Id = Convert.ToInt32(b.Element("Id").Value),
                                ChargeSlots = Convert.ToInt32(b.Element("ChargeSlots").Value),
                                Name = b.Element("Name").Value,
                                Latitude = Convert.ToDouble(b.Element("Latitude").Value),
                                Longitude = Convert.ToDouble(b.Element("Longitude").Value),
                            });
            return baseStations;
        }


        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of basestation</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> RequestPartListBaseStations(Predicate<BaseStation> predicate)
        {
            IEnumerable<BaseStation> baseStations;
            try
            {
                baseStations = RequestListBaseStations();
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            return baseStations.Where(baseStation => predicate(baseStation));
        }

        #endregion

        #region updateMethods

        /// <summary>
        /// the func updates name or/and sum of charge slots of a Base-Station
        /// </summary>
        /// <param name="b">the first BaseStation object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation b)
        {
            XElement baseStationsXml;
            try
            {
                baseStationsXml = XMLTools.LoadListFromXmlElement(baseStationsPath);
            }
            catch(XMLFileLoadCreateException e) { throw e; }
            XElement baseStation;
            try
            {
                baseStation = RequestXmlBaseStation(baseStationsXml, b.Id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            if (b.Name != "")
            {
                baseStation.Element("Name").Value = b.Name;
            }
            if (b.ChargeSlots > -1)
            {
                baseStation.Element("ChargeSlots").Value = ""+b.ChargeSlots;
            }
            try
            {
                XMLTools.SaveListToXmlElement(baseStationsXml, baseStationsPath);
            }
            catch(XMLFileLoadCreateException e) { throw e; }
        }

        #endregion

        /// <summary>
        /// delete base station - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBaseStation(int id)
        {
            XElement baseStationsXml;
            try
            {
                baseStationsXml = XMLTools.LoadListFromXmlElement(baseStationsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            XElement baseStation;
            try
            {
                baseStation = RequestXmlBaseStation(baseStationsXml, id);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            baseStation.Element("IsDeleted").Value = "true";
            try
            {
                XMLTools.SaveListToXmlElement(baseStationsXml, baseStationsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        #region helpFunctions

        /// <summary>
        /// create an xelement object
        /// </summary>
        /// <param name="baseStation">the first BaseStation value</param>
        /// <returns>Xelement value</returns>
        private XElement xmlBaseStation(BaseStation baseStation)
        {
            return new XElement("BaseStation",
                        new XElement("Id", ""+baseStation.Id),
                        new XElement("Name", ""+baseStation.Name),
                        new XElement("ChargeSlots", ""+baseStation.ChargeSlots),
                        new XElement("Longitude", ""+baseStation.Longitude),
                        new XElement("Latitude", ""+baseStation.Latitude),
                        new XElement("IsDeleted", "false")
                        );
        }

        /// <summary>
        /// return one element of an xelement value.
        /// </summary>
        /// <param name="root">the first Xelement value</param>
        /// <param name="id">the second int value</param>
        /// <returns>Xelement value</returns>
        public XElement RequestXmlBaseStation(XElement root, int id)
        {
                XElement baseStation = ((XElement)(from b in root.Elements()
                                    where Convert.ToInt32(b.Element("Id").Value) == id && b.Element("IsDeleted").Value == "false"
                                   select b).FirstOrDefault());
            if(baseStation == null)
                throw new UnextantException("base station");
            return baseStation;
        }

        #endregion
    }
}