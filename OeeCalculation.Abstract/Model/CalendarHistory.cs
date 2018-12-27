using System;
namespace Production.Abstract.Model
{
    public class CalendarHistory : IOperatorStationEntity, ICompletedProductionPeriod
    {
        private readonly int operatorStationId;
        private readonly int calendarHistoryId;
        private readonly int changeType;
        private readonly int calendar;
        private readonly DateTime periodStartTime;
        private readonly DateTime changeDate;
        public CalendarHistory(
            int operatorStationId,
            int calendarHistoryId,
            int changeType,
            int calendar,
            DateTime periodStartTime,
            DateTime changeDate
            )
        {
            this.operatorStationId = operatorStationId;
            this.calendarHistoryId = calendarHistoryId;
            this.changeType = changeType;
            this.calendar = calendar;
            this.periodStartTime = periodStartTime;
            this.changeDate = changeDate;
        }
        public int Id { get { return calendarHistoryId; } }
        public int OperatorStation_Id { get { return operatorStationId; } }
        public int ChangeType { get { return changeType; } }
        public int Calendar { get { return calendar; } }
        public DateTime PeriodStartTime { get { return periodStartTime; } }
        public DateTime ChangeDate { get { return changeDate; } }

        public DateTime StartTime { get { return PeriodStartTime; } }
        public DateTime? End
        {
            get
            {
                if (Calendar == 4)
                {
                    return null;
                }
                return ChangeDate;
            }
        }
    }
}
