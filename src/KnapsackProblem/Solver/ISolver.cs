using System.Collections.Generic;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Solver
{
    public interface ISolver
    {
        Result GetAnyResult(Instance instance);
    }
}
