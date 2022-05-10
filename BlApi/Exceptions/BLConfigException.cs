using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class BLConfigException : Exception
    {
        public BLConfigException(string msg) : base(msg) { }
        public BLConfigException(string msg, Exception ex) : base(msg, ex) { }
    }
}
