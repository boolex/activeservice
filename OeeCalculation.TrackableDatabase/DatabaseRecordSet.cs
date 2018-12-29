using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
using System.Linq;
using OeeCalculation.TrackableDatabase.Model;

namespace OeeCalculation.TrackableDatabase
{
    public class DatabaseRecordSet : IDatabaseRecordSet
    {
        private readonly IEnumerable<CalendarHistoryTrackable> calendars;
        private readonly IEnumerable<DowntimeOccasionTrackable> stops;
        private readonly IEnumerable<OrderTrackable> orders;
        private readonly IEnumerable<BatchTrackable> batches;
        private readonly IEnumerable<PUTimeEndTrackable> unitEnded;
        private readonly IEnumerable<PUTimeStart> unitStarted;
        private readonly IEnumerable<PUTimeScrapped> unitScrapped;
        public DatabaseRecordSet()
        {
            this.calendars = new List<CalendarHistoryTrackable>();
            this.stops = new List<DowntimeOccasionTrackable>();
            this.orders = new List<OrderTrackable>();
            this.batches = new List<BatchTrackable>();
            this.unitEnded = new List<PUTimeEndTrackable>();
            this.unitStarted = new List<PUTimeStart>();
            this.unitScrapped = new List<PUTimeScrapped>();
        }
        public DatabaseRecordSet(
            IEnumerable<CalendarHistoryTrackable> calendars,
            IEnumerable<DowntimeOccasionTrackable> stops,
            IEnumerable<OrderTrackable> orders,
            IEnumerable<BatchTrackable> batches,
            IEnumerable<PUTimeEndTrackable> unitEnded,
            IEnumerable<PUTimeStart> unitStarted,
            IEnumerable<PUTimeScrapped> unitScrapped
            )
        {
            this.calendars = calendars;
            this.stops = stops;
            this.orders = orders;
            this.batches = batches;
            this.unitEnded = unitEnded;
            this.unitStarted = unitStarted;
            this.unitScrapped = unitScrapped;
        }
        public IEnumerable<PUTimeEnd> EndedUnits { get { return unitEnded; } }
        public IEnumerable<PUTimeStart> StartedUnits { get { return unitStarted; } }
        public IEnumerable<PUTimeScrapped> ScrappedUnits { get { return unitScrapped; } }
        public IEnumerable<OrderBatch> Batches { get { return batches; } }
        public IEnumerable<Order> Orders { get { return orders; } }
        public IEnumerable<DowntimeOccasion> Stops { get { return stops; } }
        public IEnumerable<CalendarHistory> Calendars { get { return calendars; } }

        private List<ITrackable> events;
        public IEnumerable<ITrackable> getEvents()
        {
            var enumerators = new List<IEnumerator<ITrackable>> {
                    (IEnumerator<ITrackable>)calendars.GetEnumerator(),
                    (IEnumerator<ITrackable>)orders.GetEnumerator(),
                    (IEnumerator<ITrackable>)batches.GetEnumerator(),
                    (IEnumerator<ITrackable>)stops.GetEnumerator(),
                    //(IEnumerator<ITrackable>)StartedUnits.GetEnumerator(),
                    (IEnumerator<ITrackable>)unitEnded.GetEnumerator(),
                   // (IEnumerator<ITrackable>)ScrappedUnits.GetEnumerator()
                };

            var emptyEnumerators = new List<IEnumerator<ITrackable>>();
            enumerators.ForEach(x => { if (!x.MoveNext()) { emptyEnumerators.Add(x); } });
            emptyEnumerators.ForEach(x => enumerators.Remove(x));
            while (enumerators.Count > 0)
            {
                var enumerator = NextEnumerator(enumerators);
                yield return enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    enumerators.Remove(enumerator);
                }
            }
        }
        public IEnumerable<ITrackable> Events
        {
            get { return events ?? (events = getEvents().ToList()); }
        }
        private IEnumerator<ITrackable> NextEnumerator(List<IEnumerator<ITrackable>> enumerators)
        {
            return enumerators.First(x => x.Current.Track.Date == enumerators.Min(y => y.Current.Track.Date));
        }
    }
}
