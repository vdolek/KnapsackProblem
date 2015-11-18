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
        private Result[,] table;
        private Instance instance;

        public Result Solve(Instance instance)
        {
            this.instance = instance;
            var sumPrices = instance.Items.Sum(x => x.Price);
            table = new Result[instance.ItemCount + 1, sumPrices + 1];
            table[0, 0] = new Result(instance);

            SolveKnapsack(0, 0);

            var bestResult = new Result(instance);
            for (var n = 0; n < table.GetLength(0); ++n)
            {
                for (var p = 0; p < table.GetLength(1); ++p)
                {
                    var result = table[n, p];
                    if (p > bestResult.Price && result != null && result.Weight <= instance.Capacity)
                    {
                        bestResult = result;
                    }
                }
            }

            return bestResult;
        }

        private void SolveKnapsack(int n, int price)
        {
            if (instance.ItemCount == n)
            {
                return;
            }

            var currentResult = table[n, price];
            
            var item = instance.Items[n];

            // item is not contained
            {
                var oldResult = table[n + 1, price];
                if (oldResult == null || oldResult.Weight > currentResult.Weight)
                {
                    var newResult = currentResult.Clone();
                    table[n + 1, price] = newResult;
                    SolveKnapsack(n + 1, price);
                }
            }

            // item is contained
            {
                var newPrice = price + item.Price;
                var oldResult = table[n + 1, newPrice];
                var newWeight = currentResult.Weight + item.Weight;
                if (oldResult == null || oldResult.Weight > newWeight)
                {
                    var newResult = currentResult.Clone();
                    newResult.Price = newPrice;
                    newResult.Weight = newWeight;
                    newResult.State |= 1L << n;
                    table[n + 1, newPrice] = newResult;
                    SolveKnapsack(n + 1, newPrice);
                }
            }
        }
    }
}