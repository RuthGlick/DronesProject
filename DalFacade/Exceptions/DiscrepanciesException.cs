using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class DiscrepanciesException:Exception
    {
        public DiscrepanciesException() :base(){ }

        public DiscrepanciesException(string message) : base(message) { }

        public DiscrepanciesException(string message, Exception inner): base(message, inner) { }

        protected DiscrepanciesException(SerializationInfo info,StreamingContext context):base(info, context) { }

        public override string ToString()
        {
            if(Message == "") { return "Inconsistency between data and request"; }
            else { return Message; }
        }
    }
}
