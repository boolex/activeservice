using Production.Abstract;
namespace OeeCalculation.Computing.Statistics
{
    public class ProductionBaseAttributes : IProductionBaseAttributes
    {
        private readonly IProductionTimeStatistics times;
        private readonly IOverallAmountStatistics amount;
        public ProductionBaseAttributes(
            IProductionTimeStatistics times,
            IOverallAmountStatistics amount
            )
        {
            this.times = times;
            this.amount = amount;
        }
        public IProductionTimeStatistics Time
        {
            get { return times; }
        }
        public IOverallAmountStatistics Amount
        {
            get { return amount; }
        }      
    }
}
