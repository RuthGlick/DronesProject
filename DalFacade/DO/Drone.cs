using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"drone id: {Id} drone model: {Model}";
        }
    }
}

