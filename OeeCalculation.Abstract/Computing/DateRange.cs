using System;
namespace Production.Abstract
{
    public class DateRange
    {
        private readonly DateTime? from;
        private readonly DateTime? to;
        public DateRange(DateRange range) :
            this(
               from: range == null ? null : range.From,
               to: range == null ? null : range.To)
        {

        }
        public DateRange(
            DateTime? from = null,
            DateTime? to = null
            )
        {
            this.from = from;
            this.to = to;
        }
        public DateTime? From { get { return from; } }
        public DateTime? To { get { return to; } }

        public bool Match(ICompletedProductionPeriod period)
        {
            return
                (!from.HasValue || !period.End.HasValue || from < period.End) &&
                (!to.HasValue || to > period.StartTime);
        }
        public DateRange LimitStart(DateTime from)
        {
            return new DateRange(
                from: (!From.HasValue || From < from ? from : From),
                to: To
                );
        }

        public DateRange LimitEnd(DateTime to)
        {
            return new DateRange(
                from: From,
                to: (!To.HasValue || To > to ? to : To)
                );
        }

        public DateRange Limit(DateTime from, DateTime to)
        {
            return LimitStart(from).LimitEnd(to);
        }
    }
}
