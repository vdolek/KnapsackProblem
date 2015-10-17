using System;
using System.Numerics;
using System.Text;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts a <see cref="long"/> to a binary string.
        /// </summary>
        /// <param name="num">A <see cref="long"/>.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing a binary
        /// representation of the supplied <see cref="long"/>.
        /// </returns>
        public static string ToBinaryString(this long num)
        {
            return Convert.ToString(num, 2);
        }
    }
}
