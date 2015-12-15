using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Extensions;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners
{
    public class SimpleRunner : IRunner
    {
        private readonly IList<Instance> instances;
        private readonly ISolver solver;

        public SimpleRunner(IInstanceProvider instanceProvider, ISolver solver)
        {
            this.solver = solver;
            instances = instanceProvider.GetInstances();
        }

        public SimpleRunner(IList<Instance> instances, ISolver solver)
        {
            this.instances = instances;
            this.solver = solver;
        }

        public void Run()
        {
            var sw = Stopwatch.StartNew();

            // get results
            var runCount = 0;
            IList<Result> results;
            while (sw.ElapsedMilliseconds < 1000)
            {
                runCount++;
                results = instances.Select(instance => solver.Solve(instance)).ToList().AsReadOnly();
            }

            sw.Stop();

            // handle all results
            ////foreach (var result in results)
            ////{
            ////    HandleResult(result);
            ////}

            var time = new TimeSpan((long)(sw.ElapsedMilliseconds * 10000 / (double)runCount));

            Console.WriteLine($"Run count:  {runCount}");
            Console.WriteLine($"Time:       {time}");
            Console.WriteLine($"Seconds:    {time.TotalSeconds}");
            Console.WriteLine();
        }

        private static string GetStringRepresentation(long num, Instance instance)
        {
            var splitted = num.ToBinaryString()
                .PadLeft(instance.ItemCount, '0')
                .Reverse()
                .Select(ch => new string(new[] { ch, ' ' }));
            var res = string.Join(string.Empty, splitted);
            res = res.Substring(0, res.Length - 1);
            return res;
        }

        private void HandleResult(Result result)
        {
            var itemsStr = GetStringRepresentation(result.State, result.Instance);
            Console.WriteLine($"ID:{result.Instance.Id}\tP:{result.Price}\tW:{result.Weight}\tI:[{itemsStr}]");
        }
    }
}