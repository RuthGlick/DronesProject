using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct DroneCharge : IComparable<DroneCharge>
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }

        public override string ToString()
        {
            return $"dronecharge - station id: {StationId}";
        }

        /// <summary>
        /// compare between 2 DroneCharge objects
        /// </summary>
        /// <param name="other">first object DroneCharge</param>
        /// <returns>int</returns>
        public int CompareTo(DroneCharge other)

        {
            if (this.DroneId < other.DroneId)
            {
                return 1;
            }
            else if (this.DroneId > other.DroneId)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }
}

