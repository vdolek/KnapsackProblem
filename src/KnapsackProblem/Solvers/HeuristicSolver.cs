using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by simple heuristic.
    /// </summary>
    public class HeuristicSolver : ISolver
    {
        public Result Solve(Instance instance)
        {
            var ordered = instance.Items.OrderByDescending(item => ((double)item.Price) / item.Weight);

            var result = new Result(instance);

            foreach (var item in ordered)
            {
                var newWeight = result.Weight + item.Weight;

                if (newWeight > instance.Capacity)
                {
                    continue;
                }

                result.Weight = newWeight;
                result.Price += item.Price;
                result.State |= 1L << item.Index;
            }

            return result;
        }
    }
}