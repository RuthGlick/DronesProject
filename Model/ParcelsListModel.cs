using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PO;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace PL.Model
{
    public class ParcelsListModel:DependencyObject
    {
        static readonly ParcelsListModel instance = new ParcelsListModel();
        internal static ParcelsListModel Instance { get => instance; }

        BlApi.IBL bl;
        ListCollectionView parcelView;

        /// <summary>
        /// init bl, and 2 lists
        /// </summary>
        private ParcelsListModel()
        {
            try
            {
               
                bl = BlApi.BlFactory.GetBl();
                Parcels = new ObservableCollection<ParcelForList>();
                ParcelView = new ListCollectionView(Parcels);
                lock (bl)
                {
                    foreach (var item in bl.RequestListParcels())
                        Parcels.Add(convertParcel(item));
                }
            }
            catch (BO.BLConfigException e)
            {
                throw new PO.BLConfigException(e.Message, e);
            }
            catch (BO.DiscrepanciesException e)
            {
                throw new PO.DiscrepanciesException(e.Message, e);
            }
            catch (BO.UnextantException e)
            {
                throw new PO.UnextantException(e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw new PO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }
        }


        /// <summary>
        /// convert ParcelForList BO to PO
        /// </summary>
        /// <param name="item">first BO.ParcelForList object</param>
        /// <returns>PO.ParcelForList</returns>
        private PO.ParcelForList convertParcel(BO.ParcelForList item)
        {
            PO.ParcelForList parcelConverted = new PO.ParcelForList()
            {
                Id = item.Id,
                SenderName = item.SenderName,
                TargetName = item.TargetName,
                Weight = (PO.Enum.WeightCategories)item.Weight,
                Priority = (PO.Enum.Priorities)item.Priority,
                Status = (PO.Enum.DeliveryStatus)item.Status
            };
            return parcelConverted;
        }

        public readonly DependencyProperty listParcels =
          DependencyProperty.Register(nameof(Parcels), typeof(ObservableCollection<PO.ParcelForList>), typeof(ParcelsListModel), new PropertyMetadata(null));

        public ObservableCollection<PO.ParcelForList> Parcels
        {
            get { return (ObservableCollection<PO.ParcelForList>)GetValue(listParcels); }
            private set { SetValue(listParcels, value); }
        }

        /// <summary>
        /// RefreSh the 2 lists 
        /// </summary>
        public void RefreShParcels()
        {
            Parcels.Clear();
            try
            {
                lock (bl)
                {
                    foreach (var item in bl.RequestListParcels())
                        Parcels.Add(convertParcel(item));
                }
                ParcelView.Refresh();
            }
            catch (BO.DiscrepanciesException e)
            {
                throw new PO.DiscrepanciesException(e.Message, e);
            }
            catch (BO.UnextantException e)
            {
                throw new PO.UnextantException(e.Message, e);
            }
            catch (BO.XMLFileLoadCreateException e)
            {
                throw new PO.XMLFileLoadCreateException(e.xmlFilePath, e.Message, e);
            }

        }

        public ListCollectionView ParcelView
        {
            get => parcelView;
            set => parcelView = value;
        }
    }
}
