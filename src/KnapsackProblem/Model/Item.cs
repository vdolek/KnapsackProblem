namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Item
    {
        public int Index { get; set; }

        public int Weight { get; set; }

        public int Price { get; set; }

        public Item()
        { }

        public Item(int weight, int price)
        {
            Weight = weight;
            Price = price;
        }
    }
}