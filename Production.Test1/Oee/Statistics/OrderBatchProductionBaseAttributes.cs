namespace Production.Test1.Oee.Statistics
{
    using Production.Abstract.Model;
    using Production.Abstract;
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
