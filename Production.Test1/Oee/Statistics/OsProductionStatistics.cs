using Production.Abstract;
using System;
namespace Production.Test1.Oee.Statistics
{
    public class OsProductionStatistics : IOsProductionStatistics
    {
        private readonly TimeSpan scheduledTime;
        public OsProductionStatistics(
            TimeSpan scheduledTime
            )
        {
            this.scheduledTime = scheduledTime;
        }
        public TimeSpan ScheduledTime
        {
            get { return scheduledTime; }
        }
    }
}
