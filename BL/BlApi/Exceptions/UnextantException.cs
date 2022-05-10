using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class UnextantException : Exception
    {
        public string item { get; private set; }

        public UnextantException() : base() {
            this.item = "item";
        }

        public UnextantException(string message) : base(message) {
            this.item = message;
        }

        public UnextantException(string message, Exception inner) : base(message, inner) {
            this.item = message;
        }

        protected UnextantException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString()
        {
            return "The "+item+" does not exist in the data system";
        }
    }
}
