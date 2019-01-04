using Production.Abstract;
namespace OeeCalculation.Computing.Statistics
{
    public class ProductionStatistics : IProductionStatistics
    {
        private readonly float? availability;
        private readonly float? quality;
        private readonly float? performance;
        private readonly float? oee;
        private readonly IProductionTimeMetrics metrics;
        public ProductionStatistics(
              float? availability,
              float? quality,
              float? performance,
              float? oee,
              IProductionTimeMetrics metrics
            )
        {
            this.availability = availability;
            this.quality = quality;
            this.performance = performance;
            this.oee = oee;
            this.metrics = metrics;
        }
        public float? Availability
        {
            get { return availability; }
        }
        public float? Quality
        {
            get { return quality; }
        }
        public float? Performance
        {
            get { return performance; }
        }
        public float? OEE
        {
            get { return oee; }
        }
        public IProductionTimeMetrics Metrics
        {
            get { return metrics; }
        }
    }
}
