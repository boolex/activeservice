using Production.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
namespace OeeCalculation.Computing.Statistics
{
    public class LossStatistics : ILossStatistics
    {
        private readonly TimeSpan plan;
        private readonly TimeSpan avail;
        private readonly TimeSpan speed;
        private readonly TimeSpan rework;
        private readonly TimeSpan system;
        public LossStatistics(IEnumerable<ILossStatistics> e)
        {
            if (e != null)
            {
                plan = TimeSpan.FromSeconds(e.Sum(x => x.Planned.TotalSeconds));
                avail = TimeSpan.FromSeconds(e.Sum(x => x.Availability.TotalSeconds));
                speed = TimeSpan.FromSeconds(e.Sum(x => x.Speed.TotalSeconds));
                rework = TimeSpan.FromSeconds(e.Sum(x => x.Rework.TotalSeconds));
                system = TimeSpan.FromSeconds(e.Sum(x => x.System.TotalSeconds));
            }
        }
        public LossStatistics(ILossStatistics s)
        {
            if (s != null)
            {
                plan = s.Planned;
                avail = s.Availability;
                speed = s.Speed;
                rework = s.Rework;
                system = s.System;
            }
        }
        public LossStatistics(
            TimeSpan plan,
            TimeSpan avail,
            TimeSpan speed,
            TimeSpan rework,
            TimeSpan system
            )
        {
            this.plan = plan;
            this.avail = avail;
            this.speed = speed;
            this.rework = rework;
            this.system = system;
        }
        public LossStatistics(
           Dictionary<int, TimeSpan> loss,
           Dictionary<int, TimeSpan> active = null)
        {
            if (loss != null)
            {
                plan = GetLossFromDictionary(1, loss, active);
                avail = GetLossFromDictionary(2, loss, active);
                speed = GetLossFromDictionary(3, loss, active);
                rework = GetLossFromDictionary(4, loss, active);
                system = GetLossFromDictionary(5, loss, active);
            }
        }
        private TimeSpan GetLossFromDictionary(
            int type,
            Dictionary<int, TimeSpan> loss,
            Dictionary<int, TimeSpan> active = null)
        {
            return (loss.ContainsKey(type) ? loss[type] : TimeSpan.Zero) +
                ((active != null && active.ContainsKey(type)) ? active[type] : TimeSpan.Zero);
        }
        public TimeSpan Planned
        {
            get { return plan; }
        }
        public TimeSpan Availability
        {
            get { return avail; }
        }
        public TimeSpan Speed
        {
            get { return speed; }
        }
        public TimeSpan Rework
        {
            get { return rework; }
        }
        public TimeSpan System
        {
            get { return system; }
        }
    }
}
