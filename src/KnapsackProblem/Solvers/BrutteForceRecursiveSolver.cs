using System.Collections.Generic;
using System.Linq;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    public class BrutteForceRecursiveSolver : ISolver
    {
        public Instance Instance { get; set; }

        public Result GetAnyResult(Instance instance)
        {
            Instance = instance;

            var inst = new InstanceInner
            {
                Items = instance.Items,
                Capacity = instance.Capacity
            };

            var res = SolveKnapsack(inst);

            var result = new Result
            {
                Instance = instance,
                Price = res,
                Weight = 0,
                State = 0
            };
            return result;
        }

        private int SolveKnapsack(InstanceInner instance)
        {
            if (!instance.Items.Any())
            {
                ////return new Result
                ////{
                ////    Instance = null, // TODO
                ////    Price = 0,
                ////    Weight = 0,
                ////    State = 0 // TODO
                ////};
                return 0;
            }

            var item = instance.Items.First();

            // item is not contained
            var instanceWithoutItem = instance;
            instanceWithoutItem.Items = instance.Items.Skip(1);
            var priceWithoutItem = SolveKnapsack(instanceWithoutItem);

            // item is contained
            var newInstance2 = instance;
            newInstance2.Items = instance.Items.Skip(1);
            newInstance2.Capacity -= item.Weight;
            var price2 = SolveKnapsack(newInstance2) + item.Price;

            // which is better?
            if (priceWithoutItem > price2 || newInstance2.Capacity < 0)
            {
                return priceWithoutItem;
            }
            else
            {
                return price2;
            }
        }
        
        private struct InstanceInner
        {
            public IEnumerable<Item> Items { get; set; }

            public int Capacity { get; set; }
        }
    }
}