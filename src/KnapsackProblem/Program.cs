using System;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers.Impl;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver.Impl;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var instanceProvider = new TestInstanceProvider();
            var brutteForceSolver = new BrutteForceSolver();
            var resultHandler = new ResultHandler();

            var runner = new Runner(instanceProvider, resultHandler, brutteForceSolver);

            runner.Run();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
