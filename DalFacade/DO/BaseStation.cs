using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Collections;


namespace DO
{
    public struct BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ChargeSlots { get; set; }
        public bool IsDeleted { get; set; } 

        /// <summary>
        /// the method override ToString method
        /// </summary>
        /// 
        public override string ToString()
        {
            return $"base-station id: {Id} name: {Name}";
        }
    }
}


