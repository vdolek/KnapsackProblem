using System;
using System.IO;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Tests
{
    [TestClass]
    public class CompareTests
    {
        private const string Path = @".\Data\knap_{0}.inst.dat";
        private static readonly int[] Sizes = { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };

        [TestMethod]
        public void CompareSimpleVsRecursive()
        {
            Console.WriteLine("ahoj");
            Compare(new BrutteForceSolver(), new BrutteForceRecursiveSolver());
        }

        [TestMethod]
        public void CompareRecursiveVsBranchAndBound()
        {
            Compare(new BrutteForceRecursiveSolver(), new BranchAndBoundSolver());
        }

        [TestMethod]
        public void CompareRecursiveVsDynamic()
        {
            Compare(new BrutteForceRecursiveSolver(), new DynamicByWeightSolver());
        }

        private static void Compare(ISolver solver1, ISolver solver2)
        {
            foreach (var size in Sizes.Take(4))
            {
                Console.WriteLine($"Size {size}:");

                var path = string.Format(Path, size);
                var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));

                var runner = new CompareRunner(instanceProvider, solver1, solver2);

                runner.Run();
            }
        }
    }
}
