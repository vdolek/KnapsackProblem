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

        private readonly double initTemperature = 100d;
        private readonly double frozenTemperature = 1d;
        private readonly double coolingCoeficient = 0.8;
        private readonly int equilibriumCoeficient = 1;

        public SimulatedAnnealingSolver()
        {
        }

        public SimulatedAnnealingSolver(double initTemperature, double frozenTemperature, double coolingCoeficient)
        {
            this.initTemperature = initTemperature;
            this.frozenTemperature = frozenTemperature;
            this.coolingCoeficient = coolingCoeficient;
        }

        public Result Solve(Instance instance)
        {
            var currentResult = new Result(instance, 0, 0, 0);
            var bestResult = currentResult;

            for (var temperature = initTemperature; IsFrozen(temperature); temperature *= coolingCoeficient)
            {
                currentResult = bestResult; // it is good to go back to best result sometimes

                for (var innerCycle = 0; Equilibrium(instance, innerCycle); ++innerCycle)
                {
                    currentResult = GetNextResult(instance, temperature, currentResult);

                    if (currentResult.Price > bestResult.Price)
                    {
                        bestResult = currentResult;
                    }
                }
            }

            return bestResult;
        }

        private bool IsFrozen(double temperature)
        {
            var res = temperature > frozenTemperature;
            return res;
        }

        private bool Equilibrium(Instance instance, int innerCycle)
        {
            var res = innerCycle < (equilibriumCoeficient * instance.Capacity);
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