using System;
using System.Collections.Generic;
using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem
{
    public class Program
    {
        private const string Path = @".\Data\Edux\knap_{0}.inst.dat";
        private static readonly int[] Sizes = { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };

        public static void Main(string[] args)
        {
            ////RunHomework1Or2();
            ////RunHomework3();
            RunHomework4();
        }

        #region HW 1 and 2

        private static void RunHomework1Or2()
        {
            try
            {
                foreach (var size in Sizes)
                {
                    ////RunHomework1(size);
                    ////RunHomework2(size);
                    RunHomework2Compare(size);
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
            var solver2 = new FptasSolver(0.5);

            var runner = new CompareRunner(instanceProvider, solver1, solver2);
            runner.Run();
        }

        #endregion

        #region HW 3

        private static void RunHomework3()
        {
            var solvers = new ISolver[]
            {
                new BrutteForceRecursiveSolver(),
                new BranchAndBoundSolver(),
                new HeuristicSolver(),
                new DynamicByPriceSolver(),
                new DynamicByWeightSolver()
            };

            var defaultParameters = new RandomParameters
            {
                NumberOfInstances = 50,
                NumberOfItems = 25,
                MaxPrice = 100,
                MaxWeight = 100,
                SumWeightToCapacityRatio = 0.5m,
                ExponentK = 1,
                SizePriority = SizePriority.Stability
            };

            //// run for different max weights
            var maxWeights = new[] { 50, 100, 150, 200 };
            foreach (var maxWeight in maxWeights)
            {
                var parameters = defaultParameters.Clone();
                parameters.MaxWeight = maxWeight;

                RunForParameters(parameters, $"Max Weight: {maxWeight}", solvers);
            }

            // run for different max prices
            var maxPrices = new[] { 50, 100, 150, 200 };
            foreach (var maxPrice in maxPrices)
            {
                var parameters = defaultParameters.Clone();
                parameters.MaxPrice = maxPrice;

                RunForParameters(parameters, $"Max Price: {maxPrice}", solvers);
            }

            // run for different ratios
            var ratios = new[] { 0.2m, 0.5m, 0.8m, 1m };
            foreach (var ratio in ratios)
            {
                var parameters = defaultParameters.Clone();
                parameters.SumWeightToCapacityRatio = ratio;

                RunForParameters(parameters, $"Sum Weight To Capacity Ratio: {ratio}", solvers);
            }

            // run for different k exponents
            var exponents = new[] { 0.2m, 0.5m, 0.8m, 1m, 2m, 5m };
            foreach (var exponent in exponents)
            {
                var parameters = defaultParameters.Clone();
                parameters.ExponentK = exponent;

                parameters.SizePriority = SizePriority.MoreSmallThings;
                RunForParameters(parameters, $"Exponent K: {exponent} (More Small Things)", solvers);

                parameters.SizePriority = SizePriority.MoreBigThings;
                RunForParameters(parameters, $"Exponent K: {exponent} (More Big Things)", solvers);
            }

            Console.ReadLine();
        }

        private static void RunForParameters(RandomParameters randomParameters, string message, IList<ISolver> solvers)
        {
            Console.WriteLine(new string('=', 64));
            Console.WriteLine(message);

            var instanceProvider = new RandomInstanceProvider(randomParameters);
            var instances = instanceProvider.GetInstances();

            Console.WriteLine(new string('=', 64));

            RunForAllRunners(instances, solvers);
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void RunForAllRunners(IList<Instance> instances, IList<ISolver> solvers)
        {
            foreach (var solver in solvers)
            {
                Console.WriteLine(solver.GetType().Name);
                Console.WriteLine(new string('-', 48));

                var runner = new CompareRunner(instances, new DynamicByWeightSolver(), solver);
                runner.Run();
            }
        }

        #endregion

        #region HW 4

        private static void RunHomework4()
        {
            var path = string.Format(Path, 40);
            var instanceProvider = new TextReaderInstanceProvider(() => new StreamReader(path));

            var exactSolver = new DynamicByPriceSolver();
            var heuristicSolver = new HeuristicSolver();

            IRunner runner;

            Console.WriteLine("Solving by heuristic");
            runner = new CompareRunner(instanceProvider, exactSolver, heuristicSolver);
            runner.Run();
            Console.WriteLine();

            Console.WriteLine("Solving by simulated annealing");
            var simulatedAnnealingSolver = new SimulatedAnnealingSolver(initTemperature: 100, frozenTemperature: 1, coolingCoeficient: 0.8, equilibriumCoeficient: 2);
            runner = new CompareRunner(instanceProvider, exactSolver, simulatedAnnealingSolver);
            runner.Run();

            Console.ReadLine();
        }

        #endregion
    }
}
