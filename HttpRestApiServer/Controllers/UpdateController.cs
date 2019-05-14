using HttpRestApiServer;
using HttpRestApiServer.Attributes;
using Lab6.Attributes;
using Lab6.FakeDatabase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Lab6.Controllers
{

    [Controller("update")]
    public class UpdateController
    {
        CarsFakeTable _carsFakeTable;
        MutantsFakeTable _mutantsFakeTable;

        public UpdateController(CarsFakeTable carsFakeTable, MutantsFakeTable mutantsFakeTable)
        {            
            _carsFakeTable = carsFakeTable;
            _mutantsFakeTable = mutantsFakeTable;
        }

        [Page(@"[0-9]+")]
        public IHttpJsonResponse Post(string table, int i, [PostData]string data)
        {
            switch (table)
            {
                case "mutants":
                    Mutant mutant = JsonConvert.DeserializeObject<Mutant>(data);
                    if (i != mutant.Id)
                        throw new HttpStatusCodeException(HttpStatusCode.BadRequest);

                    _mutantsFakeTable.Update(i, mutant);
                    break;
                case "cars":
                    Car car = JsonConvert.DeserializeObject<Car>(data);
                    if (i != car.Id)
                        throw new HttpStatusCodeException(HttpStatusCode.BadRequest);

                    _carsFakeTable.Update(i, car);
                    break;
                default:
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound);
            }

            return new HttpJsonResponse();
        }

        [Page("index")]
        public IHttpJsonResponse GetIndex()
        {
            var methodsInfo = GetType().GetMethods();
            return new HttpJsonResponse { Data = JsonConvert.SerializeObject(methodsInfo.Select(t => new { Name = t.Name }), Formatting.Indented) };
        }


        [Error(HttpStatusCode.NotFound, "Page with current index not found")]
        public IHttpJsonResponse NotFound()
        {
            var myAttribute = GetType().GetMethod("NotFound").GetCustomAttributes(true).OfType<ErrorAttribute>().FirstOrDefault();
            return new HttpJsonResponse { Data = $"<html><head><meta charset='utf8'></head><body>{myAttribute.Description}</body></html>" };
        }
    }
}