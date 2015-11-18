using System;
using System.IO;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem
{
    public class Program
    {
        private const string Path = @".\Data\knap_{0}.inst.dat";
        private static readonly int[] Sizes = { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };

        public static void Main(string[] args)
        {
            try
            {
                foreach (var size in Sizes)
                {
                    RunHomework1(size);
                    ////RunHomework2(size);
                    ////RunHomework2Compare(size);
                }

                Console.WriteLine("Done.");
                Console.ReadLine();
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine("ERROR");
                Console.Error.WriteLine(exc.Message);
            }
        }

        private static void RunHomework1(int size)
        {
            Console.WriteLine($"Size {size}:");

            var path = string.Format(Path, size);
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            var brutteForceSolver = new BrutteForceSolver();
            var heuristicSolver = new HeuristicSolver();

            var runner = new CompareRunner(instanceProvider, brutteForceSolver, heuristicSolver);
            runner.Run();
        }

        private static void RunHomework2(int size)
        {
            Console.WriteLine();
            Console.WriteLine($"Size {size}:");

            var path = string.Format(Path, size);
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            ////var solver = new BrutteForceRecursiveSolver();
            ////var solver = new BranchAndBoundSolver();
            ////var solver = new DynamicByWeightSolver();
            var solver = new DynamicByPriceSolver();

            var runner = new SimpleRunner(instanceProvider, solver);
            runner.Run();
        }

        private static void RunHomework2Compare(int size)
        {
            Console.WriteLine($"Size {size}:");

            var path = string.Format(Path, size);
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            ////var solver1 = new DynamicByPriceSolver();
            var solver1 = new DynamicByWeightSolver();
            ////var solver2 = new FptasSolver(0.1);
            ////var solver2 = new FptasSolver(0.2);
            var solver2 = new FptasSolver(0.9);

            var runner = new CompareRunner(instanceProvider, solver1, solver2);
            runner.Run();
        }
    }
}
