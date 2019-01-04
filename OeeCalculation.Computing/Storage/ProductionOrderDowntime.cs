using System;
using Production.Abstract;
using System.Linq;
using System.Collections.Generic;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Storage
{
    using OeeCalculation.Computing.Statistics;
    public class ProductionOrderDowntime : DynamicIntersectionStorage<Intersection<Shift, OrderBatch>, DowntimeOccasion>
    {
        public ProductionOrderDowntime(IDynamicStorage<Intersection<Shift, OrderBatch>> po, IDynamicStorage<DowntimeOccasion> d) : base(po, d) { }
        public ILossStatistics GetForOrderBatch(OrderBatch batch, DateRange range)
        {
            var items = History.Where(x => x.A.B == batch).GroupBy(x => x.B.LossType).ToDictionary(
                x => x.Key == null ? 2 : x.Key.Value,
                y => TimeSpan.FromSeconds(y.Sum(t => new PeriodDuration(t).GetDuration(range).TotalSeconds))
                );
            var active = new Dictionary<int, TimeSpan>();
            if (Active != null && Active.A.B == batch)
            {
                active.Add(
                    Active.B.LossType == null ? 2 : Active.B.LossType.Value,
                    new PeriodDuration(Active).GetDuration(range));
            }
            return new LossStatistics(items, active);
        }
        public TimeSpan GetNotPlanprodLoss(DateTime now)
        {
            return GetActiveLoss(1, now);
        }
        public TimeSpan GetActiveAvailabilityLoss(DateTime now)
        {
            return GetActiveLoss(2, now);
        }
        public TimeSpan GetActiveReworkLoss(DateTime now)
        {
            return GetActiveLoss(4, now);
        }
        public TimeSpan GetActiveSpeedLoss(DateTime now)
        {
            return GetActiveLoss(3, now);
        }
        public TimeSpan GetActiveSystemLoss(DateTime now)
        {
            return GetActiveLoss(5, now);
        }
        private TimeSpan GetActiveLoss(int loss, DateTime now)
        {
            if (A.Active != null)
            {
                var batchid = A.Active.B.Id;
                TimeSpan activeStopDuration = TimeSpan.Zero;
                if (B.Active != null && B.Active.LossType == loss)
                {
                    activeStopDuration = new PeriodDuration(Active).GetDuration(now);
                }
                return TimeSpan.FromSeconds(History.Where(x => x.A.B.Id == batchid && x.B.LossType == loss).Sum(x => new PeriodDuration(x).GetDuration(now).TotalSeconds)) + activeStopDuration;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
    }
}
