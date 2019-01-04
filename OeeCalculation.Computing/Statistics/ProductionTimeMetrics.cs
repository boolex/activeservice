using Production.Abstract;
using System;
namespace OeeCalculation.Computing.Statistics
{
    public class ProductionTimeMetrics : IProductionTimeMetrics
    {
        private readonly IProductionTimeStatistics time;
        private readonly IOverallAmountStatistics amount;
        private readonly TimeSpan estimated;
        private readonly TimeSpan speedCalcLoss;
        private readonly TimeSpan scrapTime;
        public ProductionTimeMetrics(
            IProductionTimeStatistics time,
            IOverallAmountStatistics amount,
            TimeSpan estimated,
            TimeSpan speedCalcLoss,
            TimeSpan scrapTime)
        {
            this.time = time;
            this.amount = amount;
            this.estimated = estimated;
            this.speedCalcLoss = speedCalcLoss;
            this.scrapTime = scrapTime;
        }
        public TimeSpan Estimated
        {
            get { return estimated; }
        }
        public TimeSpan SpeedCalcLoss
        {
            get { return speedCalcLoss; }
        }
        public TimeSpan ScrapTime
        {
            get { return scrapTime; }
        }
        public IProductionTimeStatistics Time
        {
            get { return time; }
        }
        public IOverallAmountStatistics Amount
        {
            get { return amount; }
        }
    }
}
