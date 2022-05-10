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
        /// the func Creates a Parcel
        /// </summary>
        /// <param name="blParcel">the fisrt parcel object,new params in the Parcel object for creating </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(Parcel blParcel)
        {
            try
            {
                lock (dal)
                {
                    dal.RequestCustomer(blParcel.Sender.Id);
                    dal.RequestCustomer(blParcel.Target.Id);
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
            blParcel.Drone1 = null;
            blParcel.Created = DateTime.Now;
            blParcel.Scheduled = null;
            blParcel.PickedUp = null;
            blParcel.Delivered = null;
            DO.Parcel dalParcel = new DO.Parcel();
            dalParcel.SenderId = blParcel.Sender.Id;
            dalParcel.TargetId = blParcel.Target.Id;
            dalParcel.Weight = (DO.WeightCategories)blParcel.Weight;
            dalParcel.DroneId = 0;
            dalParcel.Priority = (DO.Priorities)blParcel.Priority;
            dalParcel.Requested = blParcel.Created;
            dalParcel.Scheduled = blParcel.Scheduled;
            dalParcel.PickedUp = blParcel.PickedUp;
            dalParcel.Delivered = blParcel.Delivered;
            try
            {
                lock (dal)
                {
                    dal.CreateParcel(dalParcel);
                }
            }
            catch (DO.ExtantException e)
            {
                throw new ExtantException("parcel", e);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }

        #region requestMethods

        /// <summary>
        /// the func requests a parcel from the dal
        /// </summary>
        /// <param name="blParcel">the fisrt parcel object</param>
        /// <returns>parcel object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel RequestParcel(Parcel blParcel)
        {
            DO.Parcel parcel = new DO.Parcel();
            try
            {
                lock (dal)
                {
                    parcel = dal.RequestParcel(blParcel.Id);
                }
            }
            catch (DO.UnextantException e)
            {
                throw new UnextantException(e.item, e);
            }
            try
            {
                return convertToParcel(parcel);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// the func requests the list of the parcels from the dal
        /// </summary>
        /// <returns>IEnumerable of ParcelForList</returns>
        /// 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelForList> RequestListParcels()
        {
            List<ParcelForList> parcels = new List<ParcelForList>();
            IEnumerable<DO.Parcel> temp = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    temp = dal.RequestListParcels();
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            try
            {
                parcels = (temp.Select(item => convertParcel(item))).ToList();
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            return parcels.OrderBy(p => p.Id);
        }

        /// <summary>
        /// the func requests from the dal a list of parcels which didn't belonged 
        /// to a drone yet
        /// </summary>
        /// <returns>IEnumerable of ParcelForList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelForList> RequestListPendingParcels()
        {
            List<ParcelForList> unschuduled = new List<ParcelForList>();
            IEnumerable<DO.Parcel> temp = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    temp = dal.RequestPartListParcels(isScheduled);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            try
            {
                unschuduled = (temp.Select(item => convertParcel(item))).ToList();
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (DiscrepanciesException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            return unschuduled.OrderBy(p => p.Id);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelForList> RequestPartListParcels(Predicate<ParcelForList> predicate)
        {
            IEnumerable<DO.Parcel> parcelList = Enumerable.Empty<DO.Parcel>();
            try
            {
                lock (dal)
                {
                    parcelList = dal.RequestListParcels();
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            List<BO.ParcelForList> BOParcels = new List<BO.ParcelForList>();
            try
            {
                BOParcels = (parcelList.Select(item => convertParcel(item))).ToList();
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            return BOParcels.Where(parcel => predicate(parcel)).OrderBy(p=>p.Id);
        }

        #endregion

        /// <summary>
        /// delete Parcel if the it can be deleted
        /// </summary>
        /// <param name="id">first int value</param>
        /// <returns>bool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteParcel(int id)
        {
            Parcel boParcel = new Parcel() { Id = id };
            try
            {
                boParcel = RequestParcel(boParcel);
            }
            catch (UnextantException e)
            {
                throw e;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw e;
            }
            if (boParcel.Scheduled!=null&&boParcel.Delivered==null)
            {
                return false;
            }
            try
            {
                lock (dal)
                {
                    dal.DeleteParcel(id);
                }
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
            return true;
        }
    }
}
