using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using HttpRestApiServer;
using Lab6.Attributes;
using Lab6.FakeDatabase;


namespace Lab6.Controllers
{

    [Controller("values")]
    public class ValuesController
    {
        [Page(@"[0-9]+")]
        public IHttpJsonResponse Get(int i)
        {
            Console.WriteLine($"Controller \"Values/{i}\" return {i}");
            return new HttpJsonResponse { Data = $"<html><head><meta charset='utf8'></head><body> {i}</body></html>" };
        }

        [Page("index")]
        public IHttpJsonResponse GetIndex()
        {
            Console.WriteLine($"Controller \"Values/index\" return Not Found exception");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound);
        }

        [Error(HttpStatusCode.NotFound, "Page with current index not found")]
        public IHttpJsonResponse NotFound()
        {
            var myAttribute = GetType().GetMethod("NotFound").GetCustomAttributes(true).OfType<ErrorAttribute>().FirstOrDefault();
            return new HttpJsonResponse { Data = $"<html><head><meta charset='utf8'></head><body>{myAttribute.Description}</body></html>" };
        }
}
}
