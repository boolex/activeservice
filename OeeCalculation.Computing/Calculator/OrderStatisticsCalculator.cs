using System.Collections.Generic;
using System.Linq;
using System;
using Production.Abstract.Model;
using Production.Abstract;
namespace OeeCalculation.Computing.Calculator
{
    using OeeCalculation.Computing.Statistics;
    public class OrderStatisticsCalculator : IStatisticsCalculator
    {
        private readonly Order order;
        private readonly IEnumerable<IProductionBaseAttributes> batches;
        public OrderStatisticsCalculator(
            Order order,
            IEnumerable<IProductionBaseAttributes> batches)
        {
            this.order = order;
            this.batches = batches;
        }
        public IProductionBaseAttributes Get(DateRange range)
        {
            var loss = new LossStatistics(
                    plan: Total(batches.Select(x => x.Time.Loss.Planned)),
                    avail: Total(batches.Select(x => x.Time.Loss.Availability)),
                    speed: Total(batches.Select(x => x.Time.Loss.Speed)),
                    rework: Total(batches.Select(x => x.Time.Loss.Rework)),
                    system: Total(batches.Select(x => x.Time.Loss.System))
                    );
            var scheduled = Total(batches.Select(x => x.Time.Scheduled));
            var times = new ProductionTimeStatistics(
                scheduled: scheduled,
                planned: scheduled - loss.Planned,
                production: scheduled - loss.Planned - loss.Availability,
                run: scheduled - loss.Planned - loss.Availability - loss.Speed - loss.Rework - loss.System,
                loss: loss
                );
            return new ProductionBaseAttributes(
                times: times,
                amount: new OverallAmountStatistics(
                    total: new AmountStatistics(
                    started: batches.Sum(x => x.Amount.Total.StartedAmount),
                    ended: batches.Sum(x => x.Amount.Total.EndedAmount),
                    scrapped: batches.Sum(x => x.Amount.Total.ScrappedAmount)
                    ),
                    production: new AmountStatistics(
                    started: batches.Sum(x => x.Amount.InProduction.StartedAmount),
                    ended: batches.Sum(x => x.Amount.InProduction.EndedAmount),
                    scrapped: batches.Sum(x => x.Amount.InProduction.ScrappedAmount)
                    )
                )
                    );
        }
        private TimeSpan Total(IEnumerable<TimeSpan> periods)
        {
            return TimeSpan.FromSeconds(periods.Sum(x => x.TotalSeconds));
        }

    }
}
