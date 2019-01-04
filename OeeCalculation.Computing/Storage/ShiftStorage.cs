using System;
using System.Collections.Generic;
using System.Linq;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Storage
{
    public class ShiftStorage : DynamicStorage<Shift>
    {
        public ShiftStorage(List<Shift> history = null) : base(history) { }
        public TimeSpan GetScheduledTime(DateRange range)
        {
            return GetHistoryScheduledTime(range) + GetActiveScheduledTime(range);
        }

        private TimeSpan GetHistoryScheduledTime(DateRange range)
        {
            return
                TimeSpan.FromSeconds(
                    History.Where(x => range.Match(x))
                        .Sum(x => new PeriodDuration(x).GetDuration(range).TotalSeconds));
        }

        private TimeSpan GetActiveScheduledTime(DateRange range)
        {
            if (Active == null)
            {
                return TimeSpan.Zero;
            }
            else
            {
                return new PeriodDuration(Active).GetDuration(range);
            }
        }

        public Shift GetShift(DateTime time)
        {
            if (Active != null && Active.Start <= time)
            {
                return Active;
            }
            return History.FirstOrDefault(x => x.Start <= time && x.End >= time);
        }

        public DateTime? CurrentShiftStartTime
        {
            get
            {
                var cs = CurrentShift;
                if (cs == null)
                {
                    return null;
                }

                for (var i = History.Count - 1; i >= 0; i++)
                {
                    if (i == History.Count - 1 && Active != null && (History[i].Id != Active.Id || Active.Start - History[i].End.Value >= TimeSpan.FromHours(4)))
                    {
                        return Active.Start;
                    }
                    if (History[i].Start - History[i - 1].End.Value >= TimeSpan.FromHours(4) || History[i].Id != History[i - 1].Id)
                    {
                        return History[i].Start;
                    }
                }
                throw new ApplicationException("StartTime of current shift cannot be found");
            }
        }

        public Shift CurrentShift
        {
            get
            {
                if (Active != null)
                {
                    return Active;
                }
                var length = History.Count;
                if (length > 0 && (History[length - 1].End.Value - DateTime.Now).TotalHours < 4)
                {
                    return History[length - 1];
                }
                return null;
            }
        }
    }
}
