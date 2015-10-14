using System;
using System.IO;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers.Impl;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver.Impl;

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
            var resultHandler = new ResultHandler();

            var runner = new Runner(instanceProvider, resultHandler, brutteForceSolver);

            runner.Run();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
