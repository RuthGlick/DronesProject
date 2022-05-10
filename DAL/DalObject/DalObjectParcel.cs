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
        /// CreateParcel is a method in the DalObject class.
        /// the method adds a new parcel
        /// </summary>
        /// <param name="parcel">the first Parcel value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(Parcel parcel)
        {
            Parcel exist = ParcelsList.FirstOrDefault(p => p.Id == parcel.Id && p.IsDeleted == false);
            if (exist.Id != 0)
            {
                throw new ExtantException("parcel");
            }
            parcel.IsDeleted = false;
            parcel.Id =100000000+ParcelId++;
            ParcelsList.Add(parcel);
        }

        /// <summary>
        /// DisplayParcel is a method in the DalObject class.
        /// the method allows parcel view
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel RequestParcel(int id)
        {
            Parcel parcel = ParcelsList.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if (parcel.Id == 0)
            {
                throw new UnextantException("parcel");
            }
            return (Parcel)parcel;
        }

        /// <summary>
        /// ViewListCustomers is a method in the DalObject class.
        /// the method displays the List of parcels
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> RequestListParcels()
        {
            return ParcelsList.Where(p=>p.IsDeleted==false);
        }

        /// <summary>
        /// check every item according the predicate
        /// </summary>
        /// <param name="predicate">first item - predicate</param>
        /// <returns>ienumerable of parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> RequestPartListParcels(Predicate<Parcel> predicate)
        {
            return ParcelsList.Where(parcel => predicate(parcel)&& parcel.IsDeleted == false);
        }

        /// <summary>
        /// delete Parcel - by delete prop
        /// </summary>
        /// <param name="id">first int value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            Parcel temp;
            try
            {
                temp = RequestParcel(id);
            }
            catch (UnextantException e) { throw e; }
            ParcelsList.Remove(temp);
            temp.IsDeleted = true;
            ParcelsList.Add(temp);
        }
    }
}