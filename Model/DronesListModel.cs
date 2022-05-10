using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PO;
using System.Windows;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace PL.Model
{
    public class DronesListModel : DependencyObject
    {
        static readonly DronesListModel instance = new DronesListModel();
        internal static DronesListModel Instance { get => instance; }

        BlApi.IBL bl;
        ListCollectionView droneView;

        /// <summary>
        /// init bl, and 2 lists
        /// </summary>
        private DronesListModel()
        {
            try
            {
                bl = BlApi.BlFactory.GetBl();
                Drones = new ObservableCollection<DroneForList>();
                DroneView = new ListCollectionView(Drones);
                lock (bl)
                {
                    foreach (var item in bl.RequestListDrones())
                        Drones.Add(ConvertDrone(item));
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
        /// convert DroneForList BO to PO
        /// </summary>
        /// <param name="item">first BO.DroneForList object</param>
        /// <returns>PO.DroneForList</returns>
        private DroneForList ConvertDrone(BO.DroneForList item)
        {
            return new PO.DroneForList()
            {
                Id = item.Id,
                Model = item.Model,
                Weight = (PO.Enum.WeightCategories)item.Weight,
                Battery = item.Battery,
                Status = (PO.Enum.DroneStatuses)item.Status,
                Location = new PO.Location { Longitude = item.Location.Longitude, Latitude = item.Location.Latitude, },
                DeliveryId = item.DeliveryId,
            };
        }
        public readonly DependencyProperty listDrones =
          DependencyProperty.Register(nameof(Drones), typeof(ObservableCollection<PO.DroneForList>), typeof(DronesListModel), new PropertyMetadata(null));

        public ObservableCollection<PO.DroneForList> Drones
        {
            get { return (ObservableCollection<PO.DroneForList>)GetValue(listDrones); }
            private set { SetValue(listDrones, value); }
        }

        /// <summary>
        /// RefreSh the 2 lists 
        /// </summary>
        public void RefreshDrones()
        {
            Drones.Clear();
            lock (bl)
            {
                foreach (var item in bl.RequestListDrones())
                    Drones.Add(ConvertDrone(item));
            }
            DroneView.Refresh();
        }

        public ListCollectionView DroneView
        {
            get => droneView;
            set => droneView = value;
        }

    }
}
