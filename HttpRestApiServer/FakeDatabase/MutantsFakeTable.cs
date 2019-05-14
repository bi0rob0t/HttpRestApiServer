using HttpRestApiServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lab6.FakeDatabase
{
    public class MutantsFakeTable
    {
        List<Mutant> _mutants;
        public MutantsFakeTable()
        {
            _mutants = new List<Mutant>{
                            new Mutant
                            {
                                Id = 0,
                                Name = "LexMutant",
                                Description = "имба"
                            },

                            new Mutant
                            {
                                Id = 1,
                                Name = "LadosMutant",
                                Description = "дно"
                            },
                            new Mutant
                            {
                                Id = 2,
                                Name = "BeerMutant",
                                Description = "его никто не знает"
                            }
                    };
        }

        public void Add(Mutant mutant)
        {
            _mutants.Add(mutant);
        }

        public void Remove(int mutantId)
        {
            if (mutantId < 0 || mutantId > _mutants.Count - 1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);

            _mutants.RemoveAt(mutantId);
        }

        public void Update(int mutantId, Mutant mutant)
        {
            if (mutantId < 0 || mutantId > _mutants.Count - 1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);

            _mutants[mutantId] = mutant;
        }

        public Mutant Get(int mutantId)
        {
            if (mutantId < 0 || mutantId > _mutants.Count - 1)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound);

            return _mutants[mutantId];
        }

        public IEnumerable<Mutant> GetAll()
        {
            return _mutants;
        }
    }
}
