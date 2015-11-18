using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by FPTAS algorithm.
    /// </summary>
    public class FptasSolver : ISolver
    {
        private readonly double epsilon;

        private readonly DynamicByPriceSolver dynamicByPriceSolver = new DynamicByPriceSolver();

        public FptasSolver(double epsilon)
        {
            this.epsilon = epsilon;
        }

        public Result Solve(Instance instance)
        {
            var maxPrice = instance.Items.Max(x => x.Price);
            var k = (int)(epsilon * maxPrice / instance.ItemCount);

            // get FPTAS instance with reduced prices
            var fptasInstance = instance.Clone();
            foreach (var item in fptasInstance.Items)
            {
                item.Price /= k;
            }

            // use dynamic alg FPTAS instance (decomposition by price)
            var dynamicResult = dynamicByPriceSolver.Solve(fptasInstance);

            // calculate price with real prices (not reduced prices from fptasInstance)
            var result = new Result
            {
                Instance = instance,
                Price = instance.Items.Where(x => ((1L << x.Index) & dynamicResult.State) != 0).Sum(x => x.Price),
                Weight = dynamicResult.Weight,
                State = dynamicResult.State
            };
            return result;
        }
    }
}