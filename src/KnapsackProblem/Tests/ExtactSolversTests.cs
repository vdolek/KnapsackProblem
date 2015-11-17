using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Tests
{
    [TestClass]
    public class ExtactSolversTests
    {
        private const string Path = @".\Data\knap_10.inst.dat";

        [TestMethod]
        public void TestBruteForceLinqSolver()
        {
            TestSolver(new BrutteForceLinqSolver());
        }

        [TestMethod]
        public void TestBrutteForceRecursiveSolverSolver()
        {
            TestSolver(new BrutteForceRecursiveSolver());
        }

        private void TestSolver(ISolver solver)
        {
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(Path));
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
