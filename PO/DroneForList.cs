using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PO.Enum;

namespace PO
{
    public class DroneForList: DependencyObject, ILocate
    {
        public static readonly DependencyProperty id =
         DependencyProperty.Register("Id", typeof(int), typeof(DroneForList), new PropertyMetadata(0));
        public int Id
        {
            get { return (int)GetValue(id); }
            set { SetValue(id, value); }
        }

        public static readonly DependencyProperty model =
           DependencyProperty.Register("Model", typeof(string), typeof(DroneForList), new PropertyMetadata(string.Empty));
        public string Model
        {
            get { return (string)GetValue(model); }
            set { SetValue(model, value); }
        }

        public static readonly DependencyProperty weight =
           DependencyProperty.Register("Weight", typeof(WeightCategories), typeof(DroneForList), new PropertyMetadata((WeightCategories.HEAVY)));
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(weight); }
            set { SetValue(weight, value); }
        }

        public static readonly DependencyProperty battery =
          DependencyProperty.Register("Battery", typeof(double), typeof(DroneForList), new PropertyMetadata(0.0));
        public double Battery
        {
            get { return (double)GetValue(battery); }
            set { SetValue(battery, value); }
        }

        public static readonly DependencyProperty status =
              DependencyProperty.Register("Status", typeof(DroneStatuses), typeof(DroneForList), new PropertyMetadata(DroneStatuses.MAINTENANCE));
        public DroneStatuses Status
        {
            get { return (DroneStatuses)GetValue(status); }
            set { SetValue(status, value); }
        }

        public static readonly DependencyProperty location =
           DependencyProperty.Register("Location", typeof(Location), typeof(DroneForList), new PropertyMetadata(null));
        public Location Location
        {
            get { return (Location)GetValue(location); }
            set { SetValue(location, value); }
        }

        public static readonly DependencyProperty deliveryId =
       DependencyProperty.Register("DeliveryId", typeof(int), typeof(DroneForList), new PropertyMetadata(0));
        public int DeliveryId
        {
            get { return (int)GetValue(deliveryId); }
            set { SetValue(deliveryId, value); }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
     
        #endregion

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"Id: {Id}-----Model: {Model}-----MaxWeight: {Weight}-----" +
                $"Battery: {Battery}%-----Status: {Status}-----Location: {Location}-----" +
                $"DeliveryId: {DeliveryId}";
        }

    }
}
