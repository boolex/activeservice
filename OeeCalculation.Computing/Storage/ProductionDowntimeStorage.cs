using System;
using System.Linq;
using Production.Abstract;
using System.Collections.Generic;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Storage
{
    using OeeCalculation.Computing.Statistics;
    public class ProductionDowntimeStorage : DynamicIntersectionStorage<Shift, DowntimeOccasion>
    {
        public ProductionDowntimeStorage(IDynamicStorage<Shift> s, IDynamicStorage<DowntimeOccasion> d) : base(s, d) { }
        public TimeSpan GetAvailabilityLoss(DateTime now)
        {
            return GetLoss(2, now);
        }
        public TimeSpan GetNotPlanProdLoss(DateTime now)
        {
            return GetLoss(1, now);
        }
        private TimeSpan GetLoss(int loss, DateTime now)
        {
            return TimeSpan.FromSeconds(History.Where(x => x.StartTime < now && x.B.LossType == loss).Sum(x => new PeriodDuration(x).GetDuration(now).TotalSeconds)) +
                ((Active != null && Active.B.LossType == loss) ? new PeriodDuration(Active).GetDuration(now) : TimeSpan.Zero);
        }
        public ILossStatistics GetLossStatistics(DateRange range)
        {
            range = new DateRange(range);
            var d = History.Where(x => !x.End.HasValue || !range.From.HasValue || x.End.Value > range.From.Value).GroupBy(x => x.B.LossType).ToDictionary(x => x.Key ?? 2, y => TimeSpan.FromSeconds(y.Sum(z => new PeriodDuration(z).GetDuration(range).TotalSeconds)));
            return BuildLoss(d, range);
        }

        private ILossStatistics BuildLoss(Dictionary<int, TimeSpan> d, DateRange range)
        {
            var activePlanLoss = Active != null && (Active.B.LossType == 1) ? new PeriodDuration(Active).GetDuration(range) : TimeSpan.Zero;
            var activeAvailLoss = Active != null && (Active.B.LossType == 2 || Active.B.LossType == null) ? new PeriodDuration(Active).GetDuration(range) : TimeSpan.Zero;
            var activeSpeedLoss = Active != null && (Active.B.LossType == 3) ? new PeriodDuration(Active).GetDuration(range) : TimeSpan.Zero;
            var activeReworkLoss = Active != null && (Active.B.LossType == 4) ? new PeriodDuration(Active).GetDuration(range) : TimeSpan.Zero;
            var activeSystemLoss = Active != null && (Active.B.LossType == 5) ? new PeriodDuration(Active).GetDuration(range) : TimeSpan.Zero;
            return new LossStatistics(
                plan: (d.ContainsKey(1) ? d[1] : TimeSpan.Zero) + activePlanLoss,
                avail: (d.ContainsKey(2) ? d[2] : TimeSpan.Zero) + activeAvailLoss,
                speed: (d.ContainsKey(3) ? d[3] : TimeSpan.Zero) + activeSpeedLoss,
                rework: (d.ContainsKey(4) ? d[4] : TimeSpan.Zero) + activeReworkLoss,
                system: (d.ContainsKey(5) ? d[5] : TimeSpan.Zero) + activeSystemLoss
                );
        }

    }
}
