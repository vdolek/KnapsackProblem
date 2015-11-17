using System;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by branch & bound method.
    /// </summary>
    public class BranchAndBoundSolver : ISolver
    {
        private Instance instance;
        private int bestPrice;

        public Result GetAnyResult(Instance instance)
        {
            this.instance = instance;
            bestPrice = 0;
            
            var result = SolveKnapsack(instance.Capacity, 0, 0);
            return result;
        }

        private Result SolveKnapsack(int capacity, int n, int currentPrice)
        {
            if (instance.Items.Count == n)
            {
                return new Result(instance, 0, 0, 0);
            }

            // when i cannot create better solution
            var maxAvailablePrice = currentPrice + instance.Items.Skip(n).Sum(x => x.Price);
            if (maxAvailablePrice <= bestPrice)
            {
                return new Result(instance, 0, 0, 0);
            }

            // current item
            var item = instance.Items[n];

            // item is not contained
            var resultWithoutItem = SolveKnapsack(capacity, n + 1, currentPrice);
            var priceWithoutItem = resultWithoutItem.Price;

            // item is contained
            var capacityWithoutItem = capacity - item.Weight;
            if (capacityWithoutItem < 0)
            {
                resultWithoutItem.State = resultWithoutItem.State << 1;
                return resultWithoutItem;
            }

            var resultWithItem = SolveKnapsack(capacityWithoutItem, n + 1, currentPrice + item.Price);
            var priceWithItem = resultWithItem.Price + item.Price;

            // which is better?
            if (priceWithoutItem > priceWithItem)
            {
                if (bestPrice < priceWithoutItem)
                {
                    bestPrice = priceWithoutItem;
                }

                resultWithoutItem.State = resultWithoutItem.State << 1;
                return resultWithoutItem;
            }
            else
            {
                if (bestPrice < priceWithItem)
                {
                    bestPrice = priceWithItem;
                }

                resultWithItem.Price += item.Price;
                resultWithItem.Weight += item.Weight;
                resultWithItem.State = 1 | (resultWithItem.State << 1);
                return resultWithItem;
            }
        }
    }
}