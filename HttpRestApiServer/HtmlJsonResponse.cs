using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRestApiServer
{
    public class HttpJsonResponse
            : IHttpJsonResponse
    {
        public string Data { get; set; }
    }
}
