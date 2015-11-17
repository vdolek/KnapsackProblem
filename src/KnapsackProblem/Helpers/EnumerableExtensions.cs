using System.Collections.Generic;
using System.Linq;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GetAllPermutations<T>(this IEnumerable<T> items)
        {
            if (!items.Any())
            {
                return FromSingleItem(Enumerable.Empty<T>());
            }

            var firstItem = FromSingleItem(items.First());
            var allSubPermutations = GetAllPermutations(items.Skip(1));
            var withFirst = allSubPermutations.Select(x => firstItem.Concat(x));
            var allPermutations = withFirst.Concat(allSubPermutations);
            return allPermutations;
        }

        private static IEnumerable<T> FromSingleItem<T>(T item)
        {
            yield return item;
        }
    }
}
