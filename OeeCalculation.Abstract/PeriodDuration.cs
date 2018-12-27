using System;
namespace Production.Abstract
{
    public class PeriodDuration
    {
        private readonly ICompletedProductionPeriod period;
        public PeriodDuration(ICompletedProductionPeriod period)
        {
            this.period = period;
        }
        public TimeSpan GetDuration(DateTime now)
        {
            return GetEnd(now) - period.StartTime;
        }
        public TimeSpan GetDuration(DateRange range)
        {
            range=new DateRange(range);
            return GetDurationForRange(DateTime.Now, range.From, range.To);
        }
        public DateTime GetEnd(DateTime now)
        {
            if (period.End.HasValue)
            {
                return period.End.Value;
            }
            return now;
        }
        public TimeSpan GetDurationForRange(DateTime now, DateTime? from, DateTime? to)
        {
            DateTime start = !from.HasValue || period.StartTime > from ? period.StartTime : from.Value;
            DateTime end = !to.HasValue || now < to ? now : to.Value;
            return GetEnd(end) - start;
        }
    }
}
