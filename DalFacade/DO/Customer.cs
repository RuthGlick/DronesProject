using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// the method override ToString method
        /// </summary>
        public override string ToString()
        {
            return $"customer id: {this.Id} castomer name: {this.Name}";
        }
    }
}

