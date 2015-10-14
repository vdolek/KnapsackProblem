using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers
{
    public class TextReaderInstanceProvider : IInstanceProvider
    {
        private readonly TextReader textReader;

        public TextReaderInstanceProvider(TextReader textReader)
        {
            this.textReader = textReader;
        }

        public IList<Instance> GetInstances()
        {
            return GetInstancesInner().ToList().AsReadOnly();
        }

        public IEnumerable<Instance> GetInstancesInner()
        {
            string line;
            while ((line = textReader.ReadLine()) != null)
            {
                var splitted = line.Split(' ');

                var instance = new Instance
                {
                    Id = int.Parse(splitted[0]),
                    Capacity = int.Parse(splitted[2])
                };

                var count = int.Parse(splitted[1]);
                var items = new List<Item>(count);
                for (var i = 0; i < count; ++i)
                {
                    var item = new Item
                    {
                        Index = i,
                        Weight = int.Parse(splitted[3 + 2*i]),
                        Price = int.Parse(splitted[4 + 2*i])
                    };

                    items.Add(item);
                }

                instance.Items = items.AsReadOnly();

                yield return instance;
            }
        }
    }
}