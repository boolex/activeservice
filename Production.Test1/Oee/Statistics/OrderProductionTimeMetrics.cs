using System;
using System.Collections.Generic;
using System.Linq;
using Production.Abstract;
using Production.Abstract.Model;
namespace Production.Test1.Oee.Statistics
{
    public class OrderProductionTimeMetrics : ProductionBaseAttributes, IProductionTimeMetrics
    {
        private readonly Order order;
        public OrderProductionTimeMetrics(
            Order order,
            IProductionTimeStatistics time,
            IOverallAmountStatistics amount
            )
            : base(time, amount)
        {
            this.order = order;
        }
        public OrderProductionTimeMetrics(
            Order order,
            IEnumerable<IProductionBaseAttributes> batches
            ) :
            this(
                order: order,
                time: new ProductionTimeStatistics(
                    scheduled: TimeSpan.FromSeconds(batches.Sum(x => x.Time.Scheduled.TotalSeconds)),
                    loss: new LossStatistics(batches.Select(x => x.Time.Loss))
                    ),
                amount: new OverallAmountStatistics(batches.Select(x => x.Amount)))
        {

        }
        public TimeSpan Estimated
        {
            get
            {
                return Time.Run - SpeedCalcLoss;
            }
        }
        public TimeSpan SpeedCalcLoss
        {
            get
            {
                return Time.Run > UnitTime ? Time.Run - UnitTime : TimeSpan.Zero;
            }
        }
        private TimeSpan UnitTime
        {
            get
            {
                return TimeSpan.FromSeconds(TotalProducedAmount * order.GoalCycleTime);
            }
        }
        private float TotalProducedAmount
        {
            get
            {
                return Amount.InProduction.StartedAmount > Amount.InProduction.EndedAmount ?
                    Amount.InProduction.StartedAmount : Amount.InProduction.EndedAmount;
            }
        }
        public TimeSpan ScrapTime
        {
            get
            {
                return TimeSpan.FromSeconds(order.AmountPerUnit * Amount.InProduction.ScrappedAmount);
            }
        }
    }
}
