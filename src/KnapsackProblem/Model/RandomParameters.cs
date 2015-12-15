namespace Cz.Volek.CVUT.FIT.MIPAA.KnapsackProblem.Model
{
    public enum SizePriority
    {
        MoreSmallThings = -1,
        Stability = 0,
        MoreBigThings = 1,
    }

    public class RandomParameters
    {
        public int NumberOfItems { get; set; }

        public int NumberOfInstances { get; set; }

        public decimal SumWeightToCapacityRatio { get; set; }

        public int MaxWeight { get; set; }

        public int MaxPrice { get; set; }

        public decimal ExponentK { get; set; }

        public SizePriority SizePriority { get; set; }

        public RandomParameters Clone()
        {
            return (RandomParameters)MemberwiseClone();
        }
    }
}
