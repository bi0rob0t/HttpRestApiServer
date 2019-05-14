using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HttpRestApiServer
{

    [Serializable]
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpStatusCodeException(HttpStatusCode statusCode) { StatusCode = statusCode; }
        public HttpStatusCodeException(HttpStatusCode statusCode, string message) : base(message) { StatusCode = statusCode; }
        public HttpStatusCodeException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner) { StatusCode = statusCode; }
        protected HttpStatusCodeException(
          HttpStatusCode statusCode,
          SerializationInfo info,
          StreamingContext context) : base(info, context) { StatusCode = statusCode; }
    }
}
