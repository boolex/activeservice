using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Statistics
{
    public class OrderBatchProductionBaseAttributes : ProductionBaseAttributes
    {
        private readonly OrderBatch batch;
        public OrderBatchProductionBaseAttributes(
            OrderBatch batch,
            IProductionTimeStatistics times,
            IOverallAmountStatistics amount)
            : base(times, amount)
        {
            this.batch = batch;
        }
        public OrderBatch Batch { get { return batch; } }
    }
}
