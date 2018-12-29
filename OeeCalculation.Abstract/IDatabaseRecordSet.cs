using System.Collections.Generic;
using Production.Abstract.Model;
namespace Production.Abstract
{
    public interface IDatabaseRecordSet
    {
        IEnumerable<PUTimeEnd> EndedUnits { get; }
        IEnumerable<PUTimeStart> StartedUnits { get; }
        IEnumerable<PUTimeScrapped> ScrappedUnits { get; }
        IEnumerable<OrderBatch> Batches { get; }
        IEnumerable<Order> Orders { get; }
        IEnumerable<DowntimeOccasion> Stops { get; }
        IEnumerable<CalendarHistory> Calendars { get; }
        IEnumerable<ITrackable> Events { get; }
    }
}
