using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class IncorrectInputException : Exception
    {
        public IncorrectInputException() : base() { }

        public IncorrectInputException(string message) : base(message) { }

        public IncorrectInputException(string message, Exception inner) : base(message, inner) { }

        protected IncorrectInputException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString()
        {
            return "Invalid input!";
        }
    }
}
