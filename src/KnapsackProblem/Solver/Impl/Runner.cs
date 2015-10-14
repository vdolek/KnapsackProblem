using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver.Impl
{
    public class Runner : IRunner
    {
        private readonly IInstanceProvider instanceProvider;
        private readonly IResultHandler resultHandler;
        private readonly ISolver solver;

        public Runner(IInstanceProvider instanceProvider, IResultHandler resultHandler, ISolver solver)
        {
            this.instanceProvider = instanceProvider;
            this.resultHandler = resultHandler;
            this.solver = solver;
        }

        public void Run()
        {
            var instances = instanceProvider.GetInstances();

            var results = instances.Select(instance => solver.GetAnyResult(instance)).ToList().AsReadOnly();

            foreach (var result in results)
            {
                resultHandler.HandleResult(result);
            }
        }
    }
}