using System;
using System.Collections.Generic;
using System.Text;

namespace DO
{
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }

        public DalConfigException(string msg, Exception inner) : base(msg, inner) { }
    }
}
