using System;
using System.Collections.Generic;
using System.Linq;

namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Instance
    {
        public int Id { get; set; }

        public int Capacity { get; set; }

        public IList<Item> Items { get; set; }

        public int ItemCount => Items.Count;

        public Instance Clone()
        {
            var clone = (Instance)MemberwiseClone();
            clone.Items = Array.AsReadOnly(Items.Select(i => i.Clone()).ToArray());
            return clone;
        }
    }
}
