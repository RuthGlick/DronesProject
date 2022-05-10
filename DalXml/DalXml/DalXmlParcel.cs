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
        /// CreateParcel is a method in the DalXml class.
        /// the method adds a new parcel
        /// </summary>
        /// <param name="parcel">the first Parcel value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(Parcel parcel)
        {
            parcel.IsDeleted = false;
            List<Parcel> parcelsXml;
            try
            {
                parcelsXml = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            Parcel exist = parcelsXml.FirstOrDefault(p => p.Id == parcel.Id && p.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("parcel");
            }
            parcel.Id = 100000000 + parcelsXml.Count;
            parcelsXml.Add(parcel);
            try
            {
                XMLTools.SaveListToXmlSerializer<Parcel>(parcelsXml, parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        #region requestMethods

        /// <summary>
        /// the func returns parcel by id of parcel
        /// </summary>
        /// <param name="id">int value</param>
        /// <returns>parcel object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel RequestParcel(int id)
        {
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            Parcel parcel = list.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if (parcel.Id == 0)
            {
                throw new UnextantException("parcel");
            }
            return (Parcel)parcel;
        }

        /// <summary>
        /// RequestListParcels is a method in the DalXml class.
        /// the method returns the List of parcels
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> RequestListParcels()
        {
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
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
        /// <returns>ienumerable of parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> RequestPartListParcels(Predicate<Parcel> predicate)
        {
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            return list.Where(parcel => predicate(parcel) && parcel.IsDeleted == false);
        }

        #endregion

        #region updateMethods

        /// <summary>
        /// UpdateSupply is a method in the DalXml class.
        /// the method updates delivery of a package to the destination
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSupply(Parcel p)
        {
            Parcel temp = new Parcel();
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            temp.Delivered = p.Delivered;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer(list, parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// UpdatePickedUp is a method in the DalXml class.
        /// the method updates package assembly by drone
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePickedUp(Parcel p)
        {
            Parcel temp;
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e) { throw e; }
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            temp.PickedUp = DateTime.Now;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer(list, parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        /// <summary>
        /// UpdateScheduled is a method in the DalXml class.
        /// the method assigns a package to the drone
        /// </summary>
        /// <param name="id">int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateScheduled(Parcel p)
        {
            Parcel temp = new Parcel();
            try
            {
                temp = RequestParcel(p.Id);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            List<Parcel> list;
            try
            {
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw e;
            }
            list.Remove(temp);
            temp.Scheduled = p.Scheduled;
            temp.DroneId = p.DroneId;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer(list, parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }

        #endregion

        /// <summary>
        /// delete Parcel - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            Parcel temp;
            List<Parcel> list;
            try
            {
                temp = RequestParcel(id); 
                list = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
            catch (UnextantException e) { throw e; }
            list.Remove(temp);
            temp.IsDeleted = true;
            list.Add(temp);
            try
            {
                XMLTools.SaveListToXmlSerializer<Parcel>(list, parcelsPath);
            }
            catch (XMLFileLoadCreateException e) { throw e; }
        }
    }
}
