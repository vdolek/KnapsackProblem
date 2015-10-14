using System.Linq;
using System.Numerics;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver.Impl
{
    public class HeuristicSolver : ISolver
    {
        public Result GetAnyResult(Instance instance)
        {
            var ordered = instance.Items.OrderByDescending(item => ((double)item.Price)/item.Weight);

            var result = new Result();
            result.Instance = instance;

            foreach (var item in ordered)
            {
                var newWeight = result.Weight + item.Weight;

                if (newWeight > instance.Capacity)
                    break;

                result.Weight = newWeight;
                result.Price += item.Price;
                result.State |= BigInteger.One << item.Index;
            }

            return result;
        }
    }
}