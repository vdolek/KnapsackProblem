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
        private ResultInner[,] table;
        private Instance instance;

        public Result Solve(Instance instance)
        {
            this.instance = instance;
            var sumPrices = instance.Items.Sum(x => x.Price);
            table = new ResultInner[instance.ItemCount + 1, sumPrices + 1];
            table[0, 0] = new ResultInner();

            SolveKnapsack(0, 0);

            var bestPrice = 0;
            var bestResult = new ResultInner();
            for (var n = 0; n < table.GetLength(0); ++n)
            {
                for (var price = 0; price < table.GetLength(1); ++price)
                {
                    var result = table[n, price];
                    if (price > bestPrice && result != null && result.Weight <= instance.Capacity)
                    {
                        bestResult = result;
                        bestPrice = price;
                    }
                }
            }

            var res = new Result(instance, bestResult.State, bestResult.Weight, bestPrice);
            return res;
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
                    newResult.Weight = newWeight;
                    newResult.State |= 1L << n;
                    table[n + 1, newPrice] = newResult;
                    SolveKnapsack(n + 1, newPrice);
                }
            }
        }

        private class ResultInner
        {
            public long State { get; set; }

            public int Weight { get; set; }

            public ResultInner Clone()
            {
                return new ResultInner
                {
                    Weight = Weight,
                    State = State
                };
            }
        }
    }
}