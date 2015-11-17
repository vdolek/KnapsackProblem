using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by dynamic programming.
    /// 
    /// Uses brute force recursive algorithm.
    /// </summary>
    public class DynamicByWeightSolver : BrutteForceRecursiveByWeightSolver
    {
        private Result[,] results;

        protected override void Init()
        {
            results = new Result[Instance.Capacity + 1, Instance.ItemCount + 1];
        }

        protected override Result SolveKnapsack(int capacity, int n)
        {
            if (capacity < 0)
            {
                return new Result();
            }

            var result = results[capacity, n];
            if (result != null)
            {
                return result.Clone();
            }

            result = base.SolveKnapsack(capacity, n);
            results[capacity, n] = result;
            return result.Clone();
        }
    }
}