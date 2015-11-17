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

            var result = SolveKnapsack(inst);
            result.Instance = instance;
            return result;
        }

        private Result SolveKnapsack(InstanceInner instance)
        {
            if (!instance.Items.Any())
            {
                return new Result
                {
                    Instance = null, // TODO
                    Price = 0,
                    Weight = 0,
                    State = 0
                };
            }

            var item = instance.Items.First();

            // item is not contained
            var instanceWithoutItem = instance;
            instanceWithoutItem.Items = instance.Items.Skip(1);
            var resultWithoutItem = SolveKnapsack(instanceWithoutItem);
            var priceWithoutItem = resultWithoutItem.Price;

            // item is contained
            var instanceWithItem = instance;
            instanceWithItem.Items = instance.Items.Skip(1);
            instanceWithItem.Capacity -= item.Weight;
            var resultWithItem = SolveKnapsack(instanceWithItem);
            var priceWithItem = resultWithItem.Price + item.Price;

            // which is better?
            if (priceWithoutItem > priceWithItem || instanceWithItem.Capacity < 0)
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
        
        private struct InstanceInner
        {
            public IEnumerable<Item> Items { get; set; }

            public int Capacity { get; set; }
        }
    }
}