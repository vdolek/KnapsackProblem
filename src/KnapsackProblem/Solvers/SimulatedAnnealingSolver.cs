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
            var state = 0L;
            var bestState = state;
            var bestPrice = 0;

            while (temperature > FrozenTemperature) // is frozen
            {
                var innerCycle = 0;

                state = bestState; // it is good to go back to best result sometimes

                while (Equilibrium(instance, innerCycle))
                {
                    ++innerCycle;
                    
                    state = GetNextState(instance, temperature, state);
                    var newPrice = GetPrice(instance, state);

                    if (newPrice > bestPrice)
                    {
                        bestPrice = newPrice;
                        bestState = state;
                    }
                }

                // cool down
                temperature *= CoolingCoeficient;
            }

            return new Result(instance, bestState);
        }

        private bool Equilibrium(Instance instance, int innerCycle)
        {
            var res = innerCycle < (EquilibriumCoeficient * instance.Capacity);
            return res;
        }

        private long GetNextState(Instance instance, double temperature, long state)
        {
            var oldPrice = GetPrice(instance, state);

            // get random state (only fitting in knapsack)
            var newState = GetRandomState(instance, state); ;
            var newPrice = GetPrice(instance, newState);

            // when new state is better
            if (newPrice > oldPrice)
            {
                return newState;
            }
            else // when new state is worse
            {
                var random = rand.NextDouble();
                var delta = oldPrice - newPrice;
                var useWorse = random < Math.Exp(-delta / temperature);
                return useWorse ? newState : state;
            }
        }

        /// <summary>
        /// Gets random state that differs in one bit and fots in the knapsack.
        /// </summary>
        private long GetRandomState(Instance instance, long state)
        {
            long newState;
            int newWeight;
            do
            {
                newState = GetRandomStateInner(instance, state);
                newWeight = GetWeight(instance, newState);
            } while (newWeight > instance.Capacity);
            return newState;
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