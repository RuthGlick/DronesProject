using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    [Serializable]
    public class BLConfigException : Exception
    {
        public BLConfigException(string msg) : base(msg) { }
        public BLConfigException(string msg, Exception inner) : base(msg, inner) { }
    }
}
