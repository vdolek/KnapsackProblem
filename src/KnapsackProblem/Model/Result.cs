using System.Numerics;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Result
    {
        public Instance Instance { get; set; }

        public BigInteger State { get; set; }

        public int Weight { get; set; }

        public int Price { get; set; }
    }
}
