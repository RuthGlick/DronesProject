using PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Model
{
    public class BaseStationModel : INotifyPropertyChanged
    {
        static List<BaseStationModel> listWindows = new List<BaseStationModel>();

        /// <summary>
        /// create only one instance of myBaseStation
        /// </summary>
        /// <param name="b">the first BaseStation value</param>
        /// <returns>BaseStationModel object</returns>
        public BaseStationModel GetBasseStationModel(BaseStation b)
        {
            BaseStationModel exist = (listWindows.Where(m => m.myBaseStation.Id == b.Id).Select(m => m)).FirstOrDefault();
            if (exist != null)
            {
                return exist;
            }
            MyBaseStation = b;
            listWindows.Add(this);
            return this;
        }

        private PO.BaseStation myBaseStation;
        public PO.BaseStation MyBaseStation
        {
            get => myBaseStation;
            set
            {
                myBaseStation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyBaseStation)));
            }
        }

        private bool isAdd;
        public bool IsAdd
        {
            get => isAdd;
            set
            {
                isAdd = value;
                isView = !isAdd;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdd)));
            }
        }

        private bool isView;
        public bool IsView
        {
            get => isView;
            set
            {
                isView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsView)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
