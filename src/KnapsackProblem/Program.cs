﻿using System;
using System.IO;
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
                    RunForSize(size);
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

        private static void RunForSize(int size)
        {
            Console.WriteLine($"Size {size}:");

            var path = string.Format(Path, size);
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            var brutteForceSolver = new BrutteForceSolver();
            var heuristicSolver = new HeuristicSolver();

            ////var runner = new SimpleRunner(instanceProvider, brutteForceSolver);
            ////var runner = new SimpleRunner(instanceProvider, heuristicSolver);
            var runner = new CompareRunner(instanceProvider, brutteForceSolver, heuristicSolver);

            runner.Run();
        }
    }
}
