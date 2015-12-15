using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers
{
    public class RandomInstanceProvider : IInstanceProvider
    {
        private const string ExePath = @".\Data\knapgen.exe";

        private readonly RandomParameters randomParameters;

        public RandomInstanceProvider(RandomParameters randomParameters)
        {
            this.randomParameters = randomParameters;
        }

        public IList<Instance> GetInstances()
        {
            return GetInstancesInner()
                .ToList()
                .AsReadOnly();
        }

        private TextReader GetTextReader()
        {
            var p = randomParameters;
            var arguments =
                $"-n {p.NumberOfItems} -N {p.NumberOfInstances} -m {p.SumWeightToCapacityRatio} -W {p.MaxWeight} -C {p.MaxPrice} -k {p.ExponentK} -d {(int)p.SizePriority}".Replace(",", ".");

            Console.Error.WriteLine(arguments);

            var processStartInfo = new ProcessStartInfo(ExePath)
            {
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var proc = Process.Start(processStartInfo);
            var reader = new StringReader(proc.StandardOutput.ReadToEnd());
            return reader;
        }

        private IEnumerable<Instance> GetInstancesInner()
        {
            var textReader = GetTextReader();
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
                        Weight = int.Parse(splitted[3 + 2 * i]),
                        Price = int.Parse(splitted[4 + 2 * i])
                    };

                    items.Add(item);
                }

                instance.Items = items.AsReadOnly();

                yield return instance;
            }
        }
    }
}