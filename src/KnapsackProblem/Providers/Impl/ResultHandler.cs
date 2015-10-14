using System;
using System.Linq;
using System.Numerics;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Extensions;
using Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Providers.Impl
{
    public class ResultHandler : IResultHandler
    {
        public void HandleResult(Result result)
        {
            Console.WriteLine($"ID:{result.Instance.Id} P:{result.Price} W:{result.Weight} I:{GetStringRepresentaion(result.State)}");
        }

        private static string GetStringRepresentaion(BigInteger num)
        {
            var splitted = num.ToBinaryString().Reverse().Select(ch => new String(new[] {ch, ' '}));
            var res = String.Join("", splitted);
            return res;
        }
    }
}