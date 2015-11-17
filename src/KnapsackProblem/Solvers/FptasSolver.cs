using System;
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

        public Result Solve(Instance instance)
        {
            var eps = 0.5;
            var maxPrice = instance.Items.Max(x => x.Price);
            var k = eps * maxPrice / instance.ItemCount;
            var b = (int)Math.Log(k, 2);

            var newInstance = instance.Clone();
            foreach (var item in newInstance.Items)
            {
                item.Price = item.Price >> b;
            }

            var result = dynamicByPriceSolver.Solve(newInstance);
            result.Price <<= b;
            return result;
        }
    }
}