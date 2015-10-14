using System.Collections.Generic;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers
{
    public interface IInstanceProvider
    {
        IList<Instance> GetInstances();
    }
}
