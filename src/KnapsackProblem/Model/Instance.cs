using System.Collections.Generic;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Instance
    {
        public int Id { get; set; }

        public int Capacity { get; set; }

        public IList<Item> Items { get; set; }

        public int ItemCount => Items.Count;
    }
}
