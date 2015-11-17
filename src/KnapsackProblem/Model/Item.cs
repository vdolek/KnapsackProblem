namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public class Item
    {
        public Item()
        {
        }

        public Item(int weight, int price)
        {
            Weight = weight;
            Price = price;
        }

        public int Index { get; set; }

        public int Weight { get; set; }

        public int Price { get; set; }
    }
}