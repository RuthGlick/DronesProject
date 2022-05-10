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
    public class BaseStationsListModel : DependencyObject
    {
        static readonly BaseStationsListModel instance = new BaseStationsListModel();
        internal static BaseStationsListModel Instance { get => instance; }

        BlApi.IBL bl;

        ListCollectionView baseStationView;
        public ListCollectionView BaseStationView
        {
            get => baseStationView;
            set => baseStationView = value;
        }

        public readonly DependencyProperty listBaseStations =
        DependencyProperty.Register(nameof(BaseStations), typeof(ObservableCollection<PO.BaseStationForList>), typeof(BaseStationsListModel), new PropertyMetadata(null));

        public ObservableCollection<PO.BaseStationForList> BaseStations
        {
            get { return (ObservableCollection<PO.BaseStationForList>)GetValue(listBaseStations); }
            private set { SetValue(listBaseStations, value); }
        }

        /// <summary>
        /// init bl, and 2 lists
        /// </summary>
        private BaseStationsListModel()
        {
            try
            {
                bl = BlApi.BlFactory.GetBl();
                lock (bl)
                {
                    BaseStations = new ObservableCollection<BaseStationForList>();
                    BaseStationView = new ListCollectionView(BaseStations);
                    foreach (var item in bl.RequestListBaseStations())
                        BaseStations.Add(convertBaseStation(item));
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
        /// convert BaseStation BO to PO
        /// </summary>
        /// <param name="item">first BO.BaseStationForList object</param>
        /// <returns>PO.BaseStationForList</returns>
        private PO.BaseStationForList convertBaseStation(BO.BaseStationForList item)
        {
            PO.BaseStationForList baseStationConverted = new PO.BaseStationForList()
            {
                Id = item.Id,
                Name = item.Name,
                AvailableChargeSlots = item.AvailableChargeSlots,
                CatchChargeSlots = item.CatchChargeSlots,
            };
            return baseStationConverted;
        }

        /// <summary>
        /// RefreSh the 2 lists 
        /// </summary>
        public void RefreShBaseStations()
        {
            BaseStations.Clear();
            try
            {
                lock (bl)
                {
                    foreach (var item in bl.RequestListBaseStations())
                        BaseStations.Add(convertBaseStation(item));
                    BaseStationView.Refresh();
                } 
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
    }
}
