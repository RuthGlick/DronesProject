using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class ExtantException:Exception
    {
        public ExtantException() :base(){ }

        public ExtantException(string message) : base(message) { }

        public ExtantException(string message, Exception inner): base(message, inner) { }

        protected ExtantException(SerializationInfo info,StreamingContext context):base(info, context) { }

        public override string ToString()
        {
            return "The item is almost exist in the data system";
        }
    }
}
