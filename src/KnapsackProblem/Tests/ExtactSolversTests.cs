using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Tests
{
    [TestClass]
    public class ExtactSolversTests
    {
        private const string Path = @".\Data\knap_{0}.inst.dat";

        [TestMethod]
        public void TestBruteForceLinqSolver()
        {
            TestSolver(new BrutteForceLinqSolver());
        }

        [TestMethod]
        public void TestBrutteForceRecursiveSolver()
        {
            TestSolver(new BrutteForceRecursiveByWeightSolver());
        }

        [TestMethod]
        public void TestDynamicByWeightSolver()
        {
            TestSolver(new DynamicByWeightSolver());
        }

        [TestMethod]
        public void TestBranchAndBoundSolver()
        {
            TestSolver(new BranchAndBoundSolver());
        }

        private void TestSolver(ISolver solver)
        {
            TestSolverFull(solver);
            TestSolverOnlyPrice(solver);
        }

        private void TestSolverFull(ISolver solver)
        {
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(string.Format(Path, 4)));
            var brutteForceSolver = new BrutteForceSolver();

            foreach (var instance in instanceProvider.GetInstances())
            {
                var res1 = brutteForceSolver.GetAnyResult(instance);
                var res2 = solver.GetAnyResult(instance);

                Assert.AreEqual(res1.Price, res2.Price, $"Prices are not equal for instance ${instance.Id}.");
                Assert.AreEqual(res1.Weight, res2.Weight, $"Prices are not equal for instance ${instance.Id}.");
                Assert.AreEqual(res1.State, res2.State, $"Prices are not equal for instance ${instance.Id}.");
            }
        }

        private void TestSolverOnlyPrice(ISolver solver)
        {
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(string.Format(Path, 15)));
            var brutteForceSolver = new BrutteForceSolver();

            foreach (var instance in instanceProvider.GetInstances())
            {
                var res1 = brutteForceSolver.GetAnyResult(instance);
                var res2 = solver.GetAnyResult(instance);

                Assert.AreEqual(res1.Price, res2.Price, $"Prices are not equal for instance ${instance.Id}.");
            }
        }
    }
}
