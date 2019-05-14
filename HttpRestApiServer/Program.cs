using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpRestApiServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "localhost";
            int port = 80;
            HttpServerHandler httpServerContext = new HttpServerHandler(ip, port);
            httpServerContext.StartListen();
        }
    }
}
