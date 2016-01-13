using System;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by simulated annealing.
    /// </summary>
    public class SimulatedAnnealingSolver : ISolver
    {
        private readonly Random rand = new Random(1);

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
            var newResult = GetRandomResult(instance, currentResult);

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
            Result newResult;
            do
            {
                newResult = GetRandomResultInner(instance, currentResult);
            } while (newResult.Weight > instance.Capacity);

            return newResult;
        }

        /// <summary>
        /// Gets random state that differs in one bit.
        /// </summary>
        private Result GetRandomResultInner(Instance instance, Result currentResult)
        {
            var itemIndex = rand.Next(0, instance.ItemCount);
            var itemBitArray = 1L << itemIndex;
            var randomState = currentResult.State ^ itemBitArray;
            
            var newResult = currentResult.Clone();
            newResult.State = randomState;

            if ((newResult.State & itemBitArray) == 0)
            {
                newResult.Price -= instance.Items[itemIndex].Price;
                newResult.Weight -= instance.Items[itemIndex].Weight;
            }
            else
            {
                newResult.Price += instance.Items[itemIndex].Price;
                newResult.Weight += instance.Items[itemIndex].Weight;
            }

            return newResult;
        }
    }
}