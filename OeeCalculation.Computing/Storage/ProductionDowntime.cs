using Production.Abstract;
using Production.Abstract.Model;
using Production.Abstract;
using System;
namespace OeeCalculation.Computing.Storage
{
    public class ProductionDowntime : ICompletedProductionPeriod
    {
        private readonly DateTime now;
        private readonly Shift shift;
        private readonly DowntimeOccasion downtime;
        public ProductionDowntime(Shift shift, DowntimeOccasion downtime, DateTime? now)
        {
            this.shift = shift;
            this.downtime = downtime;
            this.now = now.HasValue ? now.Value : DateTime.Now;
        }

        public DateTime? End
        {
            get
            {
                if(!shift.End.HasValue && !downtime.End.HasValue)
                {
                    return null;
                }
                else if(shift.End.HasValue && downtime.End.HasValue && shift.End < downtime.End)
                {
                    return shift.End;
                }
                else if (shift.End.HasValue && downtime.End.HasValue && shift.End >= downtime.End)
                {
                    return downtime.End;
                }
                else if (shift.End.HasValue)
                {
                    return shift.End;
                }
                else
                {
                    return downtime.End;
                }                
            }
        }
        public DateTime StartTime
        {
            get
            {
                if (shift.StartTime > downtime.StartTime)
                {
                    return shift.StartTime;
                }
                else
                {
                    return downtime.StartTime;
                }
            }
        }
    }
}
