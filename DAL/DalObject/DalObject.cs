using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DalApi;
using DO;
using static DAL.DataSource;
using static DAL.DataSource.Confing;

namespace DAL
{
    internal sealed partial class DalObject:IDal
    {
        static readonly IDal instance = new DalObject();
        internal static IDal Instance { get =>instance ; } 

        /// <summary>
        /// DalObject constructor call to intilialize
        /// </summary>
        private DalObject()
        {
            Initialize();
        }
    }
}
