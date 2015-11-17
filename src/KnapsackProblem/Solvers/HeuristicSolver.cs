﻿using System.Linq;
using System.Numerics;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solvers
{
    public class HeuristicSolver : ISolver
    {
        public Result GetAnyResult(Instance instance)
        {
            var ordered = instance.Items.OrderByDescending(item => ((double)item.Price) / item.Weight);

            var result = new Result();
            result.Instance = instance;

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