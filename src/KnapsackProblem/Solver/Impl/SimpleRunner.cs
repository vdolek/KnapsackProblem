using System;
using System.Diagnostics;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver.Impl
{
    public class SimpleRunner : IRunner
    {
        private readonly IInstanceProvider instanceProvider;
        private readonly IResultHandler resultHandler;
        private readonly ISolver solver;

        public SimpleRunner(IInstanceProvider instanceProvider, IResultHandler resultHandler, ISolver solver)
        {
            this.instanceProvider = instanceProvider;
            this.resultHandler = resultHandler;
            this.solver = solver;
        }

        public void Run()
        {
            // get instances
            var instances = instanceProvider.GetInstances();

            var sw = Stopwatch.StartNew();

            var results = instances.Select(instance => solver.GetAnyResult(instance)).ToList().AsReadOnly();

            sw.Stop();

            foreach (var result in results)
            {
                resultHandler.HandleResult(result);
            }

            Console.WriteLine();
            Console.WriteLine($"Time:    {sw.Elapsed}");
            Console.WriteLine($"Seconds: {sw.Elapsed.TotalSeconds}");
        }
    }
}