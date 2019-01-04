using System;
using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Calculator
{
    using OeeCalculation.Computing.Statistics;
    public class OrderBatchStatisticsCalculator : IStatisticsCalculator
    {
        private readonly OrderBatch orderBatch;
        public OrderBatchStatisticsCalculator(
            OrderBatch orderBatch
            )
        {
            this.orderBatch = orderBatch;
        }
        public IProductionBaseAttributes Get(DateRange range)
        {
            var scheduled = GetScheduledTime(range);
            var losses = GetLosses(range);
            var loss = new LossStatistics(
                plan: losses.ContainsKey(1) ? losses[1] : TimeSpan.Zero,
                avail: (losses.ContainsKey(2) ? losses[2] : TimeSpan.Zero) +
                (losses.ContainsKey(null) ? losses[null] : TimeSpan.Zero),
                speed: losses.ContainsKey(3) ? losses[3] : TimeSpan.Zero,
                rework: losses.ContainsKey(4) ? losses[4] : TimeSpan.Zero,
                system: losses.ContainsKey(5) ? losses[5] : TimeSpan.Zero
                );
            var scrapped = GetScrappedAmount(range);
            var times = new ProductionTimeStatistics(
                scheduled: scheduled,
                planned: scheduled - loss.Planned,
                production: scheduled - loss.Planned - loss.Availability,
                run: scheduled - loss.Planned - loss.Availability - loss.Speed - loss.Rework - loss.System,
                loss: loss);

            return new ProductionBaseAttributes(
                times: times,
                amount: new OverallAmountStatistics(
                    total: new AmountStatistics(
                    started: GetStartedAmount(range),
                    ended: GetEndedAmount(range),
                    scrapped: scrapped
                ),
                production: new AmountStatistics(
                    started: GetStartedAmountInProduction(range),
                    ended: GetEndedAmountInProduction(range),
                    scrapped: scrapped
                    )
                )
            );
        }
        private TimeSpan GetScheduledTime(DateRange range)
        {
            return TimeSpan.Zero;
        }
        private Dictionary<int?, TimeSpan> GetLosses(DateRange range)
        {
            return null;
        }
        private float GetTotalProducedAmount(DateRange range)
        {
            return 0;
        }
        private float GetStartedAmount(DateRange range)
        {
            return 0;
        }
        private float GetEndedAmount(DateRange range)
        {
            return 0;
        }
        private float GetStartedAmountInProduction(DateRange range)
        {
            return 0;
        }
        private float GetEndedAmountInProduction(DateRange range)
        {
            return 0;
        }
        private float GetScrappedAmount(DateRange range)
        {
            return 0;
        }
    }
}
