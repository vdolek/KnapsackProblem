using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners
{
    public class CompareRunner : IRunner
    {
        private readonly IInstanceProvider instanceProvider;
        private readonly ISolver solver1;
        private readonly ISolver solver2;

        public CompareRunner(IInstanceProvider instanceProvider, ISolver solver1, ISolver solver2)
        {
            this.instanceProvider = instanceProvider;
            this.solver1 = solver1;
            this.solver2 = solver2;
        }

        public void Run()
        {
            // get instances
            var instances = instanceProvider.GetInstances();

            // get results
            var results1 = instances.Select(instance => solver1.GetAnyResult(instance)).ToList().AsReadOnly();
            var results2 = instances.Select(instance => solver2.GetAnyResult(instance)).ToList().AsReadOnly();

            // handle all results
            HanldeResults(results1, results2);

            Console.WriteLine();
        }

        private void HanldeResults(ReadOnlyCollection<Result> results1, ReadOnlyCollection<Result> results2)
        {
            if (results1.Count != results2.Count)
                throw new ApplicationException("Result sets are not the same size.");

            var divergences = new List<double>(results1.Count);

            for (var i = 0; i < results1.Count; ++i)
            {
                var res1 = results1[i];
                var res2 = results2[i];

                var divergence = (res1.Price - res2.Price) / (double)res1.Price;
                divergences.Add(divergence);

                Console.WriteLine($"Relative divergence: {divergence}");
            }

            Console.WriteLine();
            Console.WriteLine($"Average relative divergence: {divergences.Average(d => d)}");
        }
    }
}