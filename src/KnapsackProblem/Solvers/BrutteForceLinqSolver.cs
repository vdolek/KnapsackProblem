using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Helpers;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solver Knapsack problem by (lazy) generating all permutation by LINQ.
    /// </summary>
    public class BrutteForceLinqSolver : ISolver
    {
        public Result GetAnyResult(Instance instance)
        {
            var permutations = instance.Items.GetAllPermutations();
            var possibleSolutions = permutations.Where(x => x.Sum(item => item.Weight) <= instance.Capacity);

            var maxPrice = 0;
            var solution = Enumerable.Empty<Item>();
            foreach (var possibleSolution in possibleSolutions)
            {
                var price = possibleSolution.Sum(item => item.Price);
                if (price > maxPrice)
                {
                    maxPrice = price;
                    solution = possibleSolution;
                }
            }

            var res = new Result(instance, solution.ToArray());
            return res;
        }
    }
}