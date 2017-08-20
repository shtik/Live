using System;
using System.Net;
using System.Runtime.Serialization;

namespace ShtikLive.Clients
{
    [Serializable]
    public class UpstreamServiceException : Exception
    {
        public int StatusCode { get; }
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UpstreamServiceException()
        {
        }

        public UpstreamServiceException(string message) : base(message)
        {
        }

        public UpstreamServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        public UpstreamServiceException(int statusCode, string message) : this(message)
        {
            StatusCode = statusCode;
        }

        public UpstreamServiceException(HttpStatusCode statusCode, string message) : this((int)statusCode, message)
        {
        }

        protected UpstreamServiceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            StatusCode = info.GetInt32(nameof(StatusCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue((string) nameof(StatusCode), (int) StatusCode);
            base.GetObjectData(info, context);
        }
    }
}