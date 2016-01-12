using System.Collections.Generic;
using System.Linq;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Result
    {
        public Result()
        {
        }

        public Result(Instance instance)
        {
            Instance = instance;
        }

        public Result(Instance instance, IList<Item> items)
        {
            Instance = instance;
            State = items.Aggregate(0L, (s, item) => s | (1L << item.Index));
            Weight = items.Sum(x => x.Weight);
            Price = items.Sum(x => x.Price);
        }

        public Result(Instance instance, long state)
            : this(instance, instance.Items.Where((x, idx) => (state & (1L << idx)) != 0).ToArray())
        {
        }

        public Result(Instance instance, long state, int weight, int price)
        {
            Instance = instance;
            State = state;
            Weight = weight;
            Price = price;
        }

        public Instance Instance { get; set; }

        public long State { get; set; }

        public int Weight { get; set; }

        public int Price { get; set; }

        public Result Clone()
        {
            return (Result)MemberwiseClone();
        }
    }
}
