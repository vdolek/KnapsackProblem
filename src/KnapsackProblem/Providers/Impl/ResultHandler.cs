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
            var itemsStr = GetStringRepresentaion(result.State, result.Instance);
            Console.WriteLine($"ID:{result.Instance.Id}\tP:{result.Price}\tW:{result.Weight}\tI:[{itemsStr}]");
        }

        private static string GetStringRepresentaion(BigInteger num, Instance instance)
        {
            var splitted = num.ToBinaryString()
                .PadLeft(instance.ItemCount, '0')
                .Reverse()
                .Select(ch => new String(new[] {ch, ' '}));
            var res = String.Join("", splitted);
            res = res.Substring(0, res.Length - 1);
            return res;
        }
    }
}