using System;
using System.Runtime.Serialization;

namespace XpsPrinting
{
    [Serializable]
    public class InvalidDocumentPageException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidDocumentPageException()
        {
        }

        public InvalidDocumentPageException(string message) : base(message)
        {
        }

        public InvalidDocumentPageException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDocumentPageException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}