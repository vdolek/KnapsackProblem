using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by dynamic programming (decomposition by price).
    /// 
    /// Uses brute force recursive algorithm.
    /// </summary>
    public class DynamicByPriceSolver : ISolver
    {
        private int?[,] weights;
        private Instance instance;

        public Result Solve(Instance instance)
        {
            this.instance = instance;
            var sumPrices = instance.Items.Sum(x => x.Price);
            weights = new int?[instance.ItemCount + 1, sumPrices + 1];
            weights[0, 0] = 0;

            SolveKnapsack(0, 0);

            var bestPrice = 0;
            var bestWeight = 0;
            for (var n = 0; n < weights.GetLength(0); ++n)
            {
                for (var p = 0; p < weights.GetLength(1); ++p)
                {
                    var weight = weights[n, p];
                    if (p > bestPrice && weight.HasValue && weight <= instance.Capacity)
                    {
                        bestPrice = p;
                        bestWeight = weight.Value;
                    }
                }
            }

            // TODO result state
            var res = new Result(instance, 0, bestWeight, bestPrice);
            return res;
        }

        private void SolveKnapsack(int n, int price)
        {
            if (instance.ItemCount == n)
            {
                return;
            }

            var weight = weights[n, price].Value;
            
            var item = instance.Items[n];

            // item is not contained
            {
                var oldWeight = weights[n + 1, price];
                if (!oldWeight.HasValue || oldWeight.Value > weight)
                {
                    weights[n + 1, price] = weight;
                    SolveKnapsack(n + 1, price);
                }
            }

            // item is contained
            var newPrice = price + item.Price;
            {
                var oldWeight = weights[n + 1, newPrice];
                var newWeight = weight + item.Weight;
                if (!oldWeight.HasValue || oldWeight.Value > newWeight)
                {
                    weights[n + 1, newPrice] = newWeight;
                    SolveKnapsack(n + 1, newPrice);

                }
            }
        }
    }
}