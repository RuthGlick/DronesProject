using PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Model
{
    public class ParcelModel : INotifyPropertyChanged
    {
        static List<ParcelModel> listWindows=new List<ParcelModel>();

        /// <summary>
        /// create only one instance of MyParcel
        /// </summary>
        /// <param name="p">the first Parcel value</param>
        /// <returns>ParcelModel object</returns>
        public ParcelModel GetParcelModel(Parcel p)
        {
            ParcelModel exist = (listWindows.Where(m => m.myParcel.Id == p.Id).Select(m => m)).FirstOrDefault();
            if (exist != null)
            {
                return exist;
            }
            MyParcel = p;
            listWindows.Add(this);
            return this;
        }

        private PO.Parcel myParcel;
        public PO.Parcel MyParcel
        {
            get => myParcel;
            set
            {
                myParcel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyParcel)));
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
