using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by FPTAS algorithm.
    /// </summary>
    public class FptasSolver : ISolver
    {
        private readonly DynamicByPriceSolver dynamicByPriceSolver = new DynamicByPriceSolver();

        public Result GetAnyResult(Instance instance)
        {
            var eps = 0.1;
            var maxPrice = instance.Items.Max(x => x.Price);
            var k = eps * maxPrice / instance.ItemCount;

            foreach (var item in instance.Items)
            {
                item.Price = (int)(item.Price / k);
            }

            return dynamicByPriceSolver.GetAnyResult(instance);
        }
    }
}