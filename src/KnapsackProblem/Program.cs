using System;
using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem
{
    class Program
    {
        private const string Path = @"C:\Users\volek\OneDrive\School\FIT\Mgr\3. semestr\PAA - Problémy a algoritmy\Archiv instancí\knap_{0}.inst.dat";
        private static readonly int[] Sizes = {4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40};

        static void Main(string[] args)
        {
            foreach (var size in Sizes)
            {
                RunForSize(size);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void RunForSize(int size)
        {
            Console.WriteLine($"Size {size}:");

            var path = String.Format(Path, size);
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            var brutteForceSolver = new BrutteForceSolver();
            var heuristicSolver = new HeuristicSolver();

            //var runner = new SimpleRunner(instanceProvider, brutteForceSolver);
            //var runner = new SimpleRunner(instanceProvider, heuristicSolver);
            var runner = new CompareRunner(instanceProvider, brutteForceSolver, heuristicSolver);

            runner.Run();
        }
    }
}
