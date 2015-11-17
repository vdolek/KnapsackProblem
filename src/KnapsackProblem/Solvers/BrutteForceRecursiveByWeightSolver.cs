using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    /// <summary>
    /// Solves knapsack problem by recursion.
    /// </summary>
    public class BrutteForceRecursiveByWeightSolver : ISolver
    {
        protected Instance Instance { get; set; }

        public Result GetAnyResult(Instance instance)
        {
            this.Instance = instance;

            Init();

            var result = SolveKnapsack(instance.Capacity, 0);
            return result;
        }

        protected virtual void Init()
        {
        }

        protected virtual Result SolveKnapsack(int capacity, int n)
        {
            if (Instance.Items.Count == n)
            {
                return new Result(Instance, 0, 0, 0);
            }

            // current item
            var item = Instance.Items[n];

            // item is not contained
            var resultWithoutItem = SolveKnapsack(capacity, n + 1);
            var priceWithoutItem = resultWithoutItem.Price;

            // item is contained
            var capacityWithoutItem = capacity - item.Weight;
            var resultWithItem = SolveKnapsack(capacityWithoutItem, n + 1);
            var priceWithItem = resultWithItem.Price + item.Price;

            // which is better?
            if (priceWithoutItem > priceWithItem || capacityWithoutItem < 0)
            {
                resultWithoutItem.State = resultWithoutItem.State << 1;
                return resultWithoutItem;
            }
            else
            {
                resultWithItem.Price += item.Price;
                resultWithItem.Weight += item.Weight;
                resultWithItem.State = 1 | (resultWithItem.State << 1);
                return resultWithItem;
            }
        }
    }
}