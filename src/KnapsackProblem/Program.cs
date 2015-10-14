using System;
using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Runners;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\volek\OneDrive\School\FIT\Mgr\3. semestr\PAA - Problémy a algoritmy\Archiv instancí\knap_15.inst.dat";

            //var instanceProvider = new TestInstanceProvider();
            var instanceProvider = new TextReaderInstanceProvider(new StreamReader(path));
            var brutteForceSolver = new BrutteForceSolver();
            var heuristicSolver = new HeuristicSolver();

            //var runner = new SimpleRunner(instanceProvider, brutteForceSolver);
            //var runner = new SimpleRunner(instanceProvider, heuristicSolver);
            var runner = new CompareRunner(instanceProvider, brutteForceSolver, heuristicSolver);

            runner.Run();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
