using System;
using System.Linq;
using Production.Abstract.Model;
using Production.Abstract;

namespace Production.Test1.Oee.Calc.Storage
{
    public class ProductionOrderBatchStorage : DynamicIntersectionStorage<Shift, OrderBatch>
    {
        public ProductionOrderBatchStorage(IDynamicStorage<Shift> s, IDynamicStorage<OrderBatch> ob) : base(s, ob) { }

        public TimeSpan GetActiveScheduledTime(DateTime now)
        {
            if (Active != null)
            {
                return new PeriodDuration(Active).GetDuration(now);
            }
            return TimeSpan.Zero;
        }
        public TimeSpan GetScheduledTime(DateTime now)
        {
            return TimeSpan.FromSeconds(History.Where(x => x.StartTime < now).Sum(x => new PeriodDuration(x).GetDuration(now).TotalSeconds)) + GetActiveScheduledTime(now);
        }
    }
}
