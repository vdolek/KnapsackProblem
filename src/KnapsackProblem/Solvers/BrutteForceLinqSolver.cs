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
        public Result Solve(Instance instance)
        {
            var permutations = instance.Items.GetAllPermutations();
            var possibleSolutions = permutations
                .Select(perm => perm.ToArray())
                .Select(perm => new
                {
                    permutation = perm,
                    weight = perm.Sum(x => x.Weight),
                    price = perm.Sum(x => x.Price)
                })
                .Where(x => x.weight <= instance.Capacity);

            var maxPrice = 0;
            var solution = Enumerable.Empty<Item>().ToArray();
            foreach (var possibleSolution in possibleSolutions)
            {
                var price = possibleSolution.price;
                if (price > maxPrice)
                {
                    maxPrice = price;
                    solution = possibleSolution.permutation;
                }
            }

            var res = new Result(instance, solution);
            return res;
        }
    }
}