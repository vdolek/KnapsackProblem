using System;
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
            var bestResult = new Result(instance, state);

            while (temperature > FrozenTemperature) // is frozen
            {
                var innerCycle = 0;

                while (Equilibrium(instance, innerCycle))
                {
                    ++innerCycle;
                    
                    state = GetNextState(instance, temperature, state);
                    var newResult = new Result(instance, state);

                    if (newResult.Price > bestResult.Price)
                    {
                        bestResult = newResult;
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

        private long GetNextState(Instance instance, double temperature, long state)
        {
            var oldResult = new Result(instance, state);

            // get random state (only fitting in knapsack)
            var newState = GetRandomState(instance, state); ;
            var newResult = new Result(instance, newState);

            // when new state is better
            if (newResult.Price > oldResult.Price)
            {
                return newState;
            }
            else // when new state is worse
            {
                var random = rand.NextDouble();
                var delta = oldResult.Price - newResult.Price;
                var useWorse = random < Math.Exp(-delta / temperature);
                //Console.WriteLine(useWorse);
                return useWorse ? newState : state;
            }
        }

        /// <summary>
        /// Gets random state that differs in one bit and fots in the knapsack.
        /// </summary>
        private long GetRandomState(Instance instance, long state)
        {
            long newState;
            Result newResult;
            do
            {
                newState = GetRandomStateInner(instance, state);
                newResult = new Result(instance, newState);
            } while (newResult.Weight > instance.Capacity);
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
    }
}