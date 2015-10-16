using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Extensions;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners
{
    public class SimpleRunner : IRunner
    {
        private readonly IInstanceProvider instanceProvider;
        private readonly ISolver solver;

        public SimpleRunner(IInstanceProvider instanceProvider, ISolver solver)
        {
            this.instanceProvider = instanceProvider;
            this.solver = solver;
        }

        public void Run()
        {
            // get instances
            var instances = instanceProvider.GetInstances();

            var sw = Stopwatch.StartNew();

            // get results
            var results = instances.Select(instance => solver.GetAnyResult(instance)).ToList().AsReadOnly();

            sw.Stop();

            // handle all results
            foreach (var result in results)
            {
                HandleResult(result);
            }

            Console.WriteLine();
            Console.WriteLine($"Time:    {sw.Elapsed}");
            Console.WriteLine($"Seconds: {sw.Elapsed.TotalSeconds}");
        }

        private void HandleResult(Result result)
        {
            var itemsStr = GetStringRepresentaion(result.State, result.Instance);
            Console.WriteLine($"ID:{result.Instance.Id}\tP:{result.Price}\tW:{result.Weight}\tI:[{itemsStr}]");
        }

        private static string GetStringRepresentaion(long num, Instance instance)
        {
            var splitted = num.ToBinaryString()
                .PadLeft(instance.ItemCount, '0')
                .Reverse()
                .Select(ch => new String(new[] { ch, ' ' }));
            var res = String.Join("", splitted);
            res = res.Substring(0, res.Length - 1);
            return res;
        }
    }
}