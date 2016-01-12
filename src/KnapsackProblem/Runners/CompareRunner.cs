using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners
{
    public class CompareRunner : IRunner
    {
        private readonly IList<Instance> instances;
        private readonly ISolver exactSolver;
        private readonly ISolver solver;

        public CompareRunner(IInstanceProvider instanceProvider, ISolver exactSolver, ISolver solver)
        {
            this.exactSolver = exactSolver;
            this.solver = solver;

            instances = instanceProvider.GetInstances();
        }

        public CompareRunner(IList<Instance> instances, ISolver exactSolver, ISolver solver)
        {
            this.instances = instances;
            this.exactSolver = exactSolver;
            this.solver = solver;
        }

        public void Run()
        {
            // get results
            IList<Result> exactResults = null;
            var sw1 = Stopwatch.StartNew();
            var exactRunCount = 0;
            while (sw1.ElapsedMilliseconds < 10)
            {
                ++exactRunCount;
                exactResults = instances.Select(instance => exactSolver.Solve(instance)).ToList().AsReadOnly();
            }

            sw1.Stop();

            ReadOnlyCollection<Result> results = null;
            var sw2 = Stopwatch.StartNew();
            var runCount = 0;
            while (sw2.ElapsedMilliseconds < 1000)
            {
                ++runCount;
                results = instances.Select(instance => solver.Solve(instance)).ToList().AsReadOnly();
            }

            sw1.Stop();

            // handle all results
            var relativeDivergences =
                exactResults
                    .Zip(results, (er, r) => new { er, r })
                    .Select(i => (i.er.Price - i.r.Price) / (double)i.er.Price).ToList().AsReadOnly();
            var averageRelativeDivergence = relativeDivergences.Average(x => x);
            var maxRelativeDivergence = relativeDivergences.Max(x => x);

            ////var time1 = new TimeSpan((long)(sw1.ElapsedMilliseconds * 10000 / (double)exactRunCount));
            var time2 = new TimeSpan((long)(sw2.ElapsedMilliseconds * 10000 / (double)runCount));

            ////foreach (var x in exactResults
            ////        .Zip(results, (er, r) => new { a = er.Price, b = r.Price }))
            ////{
            ////    Console.WriteLine($"{x.a} - {x.b} = {x.a - x.b}");
            ////}

            Console.WriteLine();
            Console.WriteLine($"Average relative divergence: {averageRelativeDivergence}");
            Console.WriteLine($"    Max relative divergence: {maxRelativeDivergence}");
            ////Console.WriteLine($"             Exact Run Time: {time1}\t(run {exactRunCount} times)");
            Console.WriteLine($"                   Run Time: {time2}\t(run {runCount} times)");
            ////Console.WriteLine($"                      Ratio: {time1.Ticks / (double)time2.Ticks:P2}");
            Console.WriteLine();
        }

        private void HanldeResults(ReadOnlyCollection<Result> exactResults, ReadOnlyCollection<Result> results)
        {
            if (exactResults.Count != results.Count)
            {
                throw new ApplicationException("Result sets are not the same size.");
            }

            var divergences = new List<double>(exactResults.Count);

            for (var i = 0; i < exactResults.Count; ++i)
            {
                var exactResult = exactResults[i];
                var result = results[i];

                var divergence = (exactResult.Price - result.Price) / (double)exactResult.Price;
                divergences.Add(divergence);

                ////Console.WriteLine($"ID:{result.Instance.Id}\tExactPrice:{exactResult.Price}\tPrice:{result.Price}\tDivergence:{divergence}");
            }

            var averageRelativeDivergence = divergences.Average(d => d);
            Console.WriteLine($"Average relative divergence: {averageRelativeDivergence}");
        }
    }
}