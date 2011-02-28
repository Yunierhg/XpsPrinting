using System;
using System.Runtime.Serialization;

namespace XpsPrinting
{
    [Serializable]
    public class InvalidReturnValue : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidReturnValue()
        {
        }

        public InvalidReturnValue(string message) : base(message)
        {
        }

        public InvalidReturnValue(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidReturnValue(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}