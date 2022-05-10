using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Model
{
    public class DroneModel : DependencyObject, INotifyPropertyChanged
    {

        static List<DroneModel> listWindows = new List<DroneModel>();

        private PO.Drone myDrone;
        public PO.Drone MyDrone
        {
            get => myDrone;
            set
            {
                myDrone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyDrone)));
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

        /// <summary>
        /// create only one instance of MyDrone
        /// </summary>
        /// <param name="d">the first PO.Drone value</param>
        /// <returns>DroneModel value</returns>
        public DroneModel GetDroneModel(PO.Drone d)
        {
            DroneModel exist = (listWindows.Where(m => m.myDrone.Id == d.Id).Select(m => m)).FirstOrDefault();
            if (exist != null)
            {
                return exist;
            }
            MyDrone = d;
            listWindows.Add(this);
            return this;
        }
    }
}
