using System;
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
            var runCount = 0;
            while (sw.ElapsedMilliseconds < 1000)
            {
                runCount++;
                var results = instances.Select(instance => solver.Solve(instance)).ToList().AsReadOnly();
            }
            sw.Stop();

            // handle all results
            ////foreach (var result in results)
            ////{
            ////    HandleResult(result);
            ////}

            //var elapsed = new TimeSpan(sw.ElapsedTicks / runCount);
            var time = new TimeSpan((long)(sw.ElapsedMilliseconds * 10000 / (double)runCount));

            Console.WriteLine();
            Console.WriteLine($"Run count:  {runCount}");
            Console.WriteLine($"Time:       {time}");
            Console.WriteLine($"Seconds:    {time.TotalSeconds}");
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