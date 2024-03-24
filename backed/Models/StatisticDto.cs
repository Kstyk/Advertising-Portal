namespace ZleceniaAPI.Models
{
    public class StatisticDto
    {
        public int AmountOfContractors { get; set; } = 0;
        public int AverageOffersForOneOrder { get; set; } = 0;
        public int AmountOfOrders { get; set; } = 0;
        public decimal TotalValueOfOrders { get; set; } = 0;
    }
}
