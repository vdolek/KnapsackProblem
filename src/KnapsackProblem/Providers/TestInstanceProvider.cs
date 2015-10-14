using System.Collections.Generic;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers
{
    public class TestInstanceProvider : IInstanceProvider
    {
        public IList<Instance> GetInstances()
        {
            return GetInstancesInner().ToList().AsReadOnly();
        }

        public IEnumerable<Instance> GetInstancesInner()
        {
            yield return new Instance
            {
                Id = 9000,
                Capacity = 100,
                Items = new[]
                {
                    new Item(18, 114),
                    new Item(42, 136),
                    new Item(88, 192),
                    new Item(3, 223)
                }
            };

            yield return new Instance
            {
                Id = 9001,
                Capacity = 100,
                Items = new[]
                {
                    new Item(55, 29),
                    new Item(81, 64),
                    new Item(14, 104),
                    new Item(52, 222)
                }
            };
        }
    }
}