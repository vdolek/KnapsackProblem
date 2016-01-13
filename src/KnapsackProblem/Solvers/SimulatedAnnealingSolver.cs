using System;
using System.Collections.Generic;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by simulated annealing.
    /// </summary>
    public class SimulatedAnnealingSolver : ISolver
    {
        private readonly Random rand = new Random(0);

        private readonly double InitTemperature = 100d;
        private readonly double FrozenTemperature = 1d;
        private readonly double CoolingCoeficient = 0.8;
        private readonly int EquilibriumCoeficient = 1;

        public Result Solve(Instance instance)
        {
            var temperature = InitTemperature;
            var currentResult = new Result(instance, 0, 0, 0);
            var bestResult = currentResult;

            while (temperature > FrozenTemperature) // is frozen
            {
                var innerCycle = 0;

                currentResult = bestResult; // it is good to go back to best result sometimes

                while (Equilibrium(instance, innerCycle))
                {
                    ++innerCycle;

                    currentResult = GetNextResult(instance, temperature, currentResult);

                    if (currentResult.Price > bestResult.Price)
                    {
                        bestResult = currentResult;
                    }
                }

                // cool down
                temperature *= CoolingCoeficient;
            }

            return bestResult;
        }

        private bool Equilibrium(Instance instance, int innerCycle)
        {
            var res = innerCycle < (EquilibriumCoeficient * instance.Capacity);
            return res;
        }

        private Result GetNextResult(Instance instance, double temperature, Result currentResult)
        {
            // get random state (only fitting in knapsack)
            var newResult = GetRandomResult(instance, currentResult); ;

            // when new state is better
            if (newResult.Price > currentResult.Price)
            {
                return newResult;
            }
            else // when new state is worse
            {
                var random = rand.NextDouble();
                var delta = currentResult.Price - newResult.Price;
                var useWorse = random < Math.Exp(-delta / temperature);
                return useWorse ? newResult : currentResult;
            }
        }

        /// <summary>
        /// Gets random state that differs in one bit and fots in the knapsack.
        /// </summary>
        private Result GetRandomResult(Instance instance, Result currentResult)
        {
            long newState;
            int newWeight;
            do
            {
                newState = GetRandomStateInner(instance, currentResult.State);
                newWeight = GetWeight(instance, newState);
            } while (newWeight > instance.Capacity);

            var newPrice = GetPrice(instance, newState);
            var newResult = new Result(instance, newState, newWeight, newPrice);
            return newResult;
        }

        /// <summary>
        /// Gets random state that differs in one bit.
        /// </summary>
        private long GetRandomStateInner(Instance instance, long state)
        {
            var itemIndex = rand.Next(0, instance.ItemCount);
            var randomState = state ^ (1L << itemIndex);
            return randomState;
        }

        private static IEnumerable<Item> GetItems(Instance instance, long state)
        {
            var items = instance.Items.Where((x, idx) => (state & (1L << idx)) != 0);
            return items;
        }

        private static int GetPrice(Instance instance, long state)
        {
            var items = GetItems(instance, state);
            var price = items.Sum(x => x.Price);
            return price;
        }

        private static int GetWeight(Instance instance, long state)
        {
            var items = GetItems(instance, state);
            var weight = items.Sum(x => x.Weight);
            return weight;
        }
    }
}