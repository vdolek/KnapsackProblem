using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    public class BrutteForceSolver : ISolver
    {
        public Result GetAnyResult(Instance instance)
        {
            var result = new Result();
            result.Instance = instance;

            var max = 1L << instance.ItemCount;

            for (var i = 0L; i < max; ++i)
            {
                var partResult = Evaluate(instance, i);

                if (partResult.Weight <= instance.Capacity)
                {
                    if (partResult.Price > result.Price)
                    {
                        result.State = i;
                        result.Weight = partResult.Weight;
                        result.Price = partResult.Price;
                    }
                }
            }

            return result;
        }

        private PartResult Evaluate(Instance instance, long state)
        {
            var partResult = new PartResult();

            for (var i = 0;; ++i)
            {
                var num = 1L << i;

                if (num > state)
                {
                    break;
                }

                if ((state & num) != 0)
                {
                    partResult.Price += instance.Items[i].Price;
                    partResult.Weight += instance.Items[i].Weight;
                }
            }

            return partResult;
        }

        private class PartResult
        {
            public int Weight { get; set; }

            public int Price { get; set; }
        }
    }
}