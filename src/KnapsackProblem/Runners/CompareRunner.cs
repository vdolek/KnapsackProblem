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
        private readonly IInstanceProvider instanceProvider;
        private readonly ISolver exactSolver;
        private readonly ISolver solver;

        public CompareRunner(IInstanceProvider instanceProvider, ISolver exactSolver, ISolver solver)
        {
            this.instanceProvider = instanceProvider;
            this.exactSolver = exactSolver;
            this.solver = solver;
        }

        public void Run()
        {
            // get instances
            var instances = instanceProvider.GetInstances();

            // get results
            var sw1 = Stopwatch.StartNew();
            var exactResults = instances.Select(instance => exactSolver.GetAnyResult(instance)).ToList().AsReadOnly();
            sw1.Stop();

            var sw2 = Stopwatch.StartNew();
            var results = instances.Select(instance => solver.GetAnyResult(instance)).ToList().AsReadOnly();
            sw1.Stop();

            // handle all results
            HanldeResults(exactResults, results);

            Console.WriteLine();
            Console.WriteLine($"Exact Run Time: {sw1.Elapsed}");
            Console.WriteLine($"      Run Time: {sw2.Elapsed}");
            Console.WriteLine($"         Ratio: {sw2.ElapsedMilliseconds / (double)sw1.ElapsedMilliseconds}");
            Console.WriteLine();
        }

        private void HanldeResults(ReadOnlyCollection<Result> exactResults, ReadOnlyCollection<Result> results)
        {
            if (exactResults.Count != results.Count)
                throw new ApplicationException("Result sets are not the same size.");

            var divergences = new List<double>(exactResults.Count);

            for (var i = 0; i < exactResults.Count; ++i)
            {
                var exactResult = exactResults[i];
                var result = results[i];

                var divergence = (exactResult.Price - result.Price) / (double)exactResult.Price;
                divergences.Add(divergence);

                Console.WriteLine($"ID:{result.Instance.Id}\tExactPrice:{exactResult.Price}\tPrice:{result.Price}\tDivergence:{divergence}");
            }

            Console.WriteLine();
            Console.WriteLine($"Average relative divergence: {divergences.Average(d => d)}");
        }
    }
}