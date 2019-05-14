using HttpRestApiServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lab6.FakeDatabase
{
    public class CarsFakeTable
    {
        List<Car> _cars;
        public CarsFakeTable()
        {
            _cars = new List<Car>{
                            new Car
                            {
                                Id = 0,
                                Name = "Lexus"
                            },

                            new Car
                            {
                                Id = 1,
                                Name = "Lada"
                            },
                            new Car
                            {
                                Id = 2,
                                Name = "BMW"
                            }
                    };
        }
        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Remove(int carId)
        {
            if (carId < 0 || carId > _cars.Count -1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);
            _cars.RemoveAt(carId);
        }

        public void Update(int carId, Car car)
        {
            if (carId < 0 || carId > _cars.Count - 1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);
            _cars[carId] = car;
        }

        public Car Get(int carId)
        {
            if (carId < 0 || carId > _cars.Count - 1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);
            return _cars[carId];
        }

        public IEnumerable<Car> GetAll()
        {

            return _cars;
        }

    }
}
