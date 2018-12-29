using System.Collections.Generic;
using OeeCalculation.TrackableDatabase.Model;
using OeeCalculation.TrackableDatabase.Serialization;
using Production.Abstract;
using System.Linq;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase
{
    public class DatabaseChangeSet : IDatabaseChangeSet
    {
        private readonly byte[] data;
        public DatabaseChangeSet(byte[] data)
        {
            this.data = data;
        }
        private readonly IDatabaseRecordSet inserted;
        private readonly IDatabaseRecordSet updated;
        private readonly IDatabaseRecordSet deleted;
        public DatabaseChangeSet(
            IDatabaseRecordSet inserted,
             IDatabaseRecordSet updated,
              IDatabaseRecordSet deleted)
        {
            this.inserted = inserted;
            this.updated = updated;
            this.deleted = deleted;
        }
        public DatabaseChangeSet(
            List<CalendarHistoryTrackable> calendarsTrackable,
            List<OrderTrackable> ordersTrackable,
            List<BatchTrackable> batchesTrackable,
            List<DowntimeOccasionTrackable> stopsTrackable,
            List<PUTimeEndTrackable> unitEndedTrackable
            ) :
            this(
                inserted: new DatabaseRecordSet(
                    calendars: calendarsTrackable.Where(x => x.Track.Type == TrackingType.Inserted),
                    orders: ordersTrackable.Where(x => x.Track.Type == TrackingType.Inserted),
                    batches: batchesTrackable.Where(x => x.Track.Type == TrackingType.Inserted),
                    stops: stopsTrackable.Where(x => x.Track.Type == TrackingType.Inserted),
                    unitStarted: new List<PUTimeStart>(),
                    unitEnded: unitEndedTrackable.Where(x => x.Track.Type == TrackingType.Inserted),
                    unitScrapped: new List<PUTimeScrapped>()
                    ),
                updated: new DatabaseRecordSet(
                    calendars: calendarsTrackable.Where(x => x.Track.Type == TrackingType.Updated),
                    orders: ordersTrackable.Where(x => x.Track.Type == TrackingType.Updated),
                    batches: batchesTrackable.Where(x => x.Track.Type == TrackingType.Updated),
                    stops: stopsTrackable.Where(x => x.Track.Type == TrackingType.Updated),
                    unitStarted: new List<PUTimeStart>(),
                    unitEnded: unitEndedTrackable.Where(x => x.Track.Type == TrackingType.Updated),
                    unitScrapped: new List<PUTimeScrapped>()
                    ),
                deleted: new DatabaseRecordSet(
                    calendars: calendarsTrackable.Where(x => x.Track.Type == TrackingType.Deleted),
                    orders: ordersTrackable.Where(x => x.Track.Type == TrackingType.Deleted),
                    batches: batchesTrackable.Where(x => x.Track.Type == TrackingType.Deleted),
                    stops: stopsTrackable.Where(x => x.Track.Type == TrackingType.Deleted),
                    unitStarted: new List<PUTimeStart>(),
                    unitEnded: unitEndedTrackable.Where(x => x.Track.Type == TrackingType.Deleted),
                    unitScrapped: new List<PUTimeScrapped>()
                    )
                )
        {
        }
        public void Load()
        {
            int position = 0;
            putimeEnd = DbListSerializer<PUTimeEndTrackable>.From(data, position, out position);
            orders = DbListSerializer<OrderTrackable>.From(data, position, out position);
            batches = DbListSerializer<BatchTrackable>.From(data, position, out position);
            stops = DbListSerializer<DowntimeOccasionTrackable>.From(data, position, out position);
            calendar = DbListSerializer<CalendarHistoryTrackable>.From(data, position, out position);
        }
        public IDatabaseRecordSet Inserted { get { return inserted; } }
        public IDatabaseRecordSet Updated { get { return updated; } }
        public IDatabaseRecordSet Deleted { get { return deleted; } }
        public IEnumerable<ITrackable> Events
        {
            get
            {
                var insertedEnumerator = Inserted.Events.GetEnumerator();
                var updatedEnumerator = Updated.Events.GetEnumerator();
                var deletedEnumerator = Deleted.Events.GetEnumerator();
                var insertedState = insertedEnumerator.MoveNext();
                var updatedState = updatedEnumerator.MoveNext();
                var deletedState = deletedEnumerator.MoveNext();
                while (insertedState || updatedState || deletedState)
                {
                    if (deletedState &&
                       (!insertedState || deletedEnumerator.Current.Track.Date <= insertedEnumerator.Current.Track.Date) &&
                       (!updatedState || deletedEnumerator.Current.Track.Date <= updatedEnumerator.Current.Track.Date))
                    {
                        yield return deletedEnumerator.Current;
                        deletedState = deletedEnumerator.MoveNext();
                    }
                    else if (updatedState &&
                       (!insertedState || updatedEnumerator.Current.Track.Date <= insertedEnumerator.Current.Track.Date))
                    {
                        yield return updatedEnumerator.Current;
                        updatedState = updatedEnumerator.MoveNext();
                    }
                    else if (insertedState)
                    {
                        yield return insertedEnumerator.Current;
                        insertedState = insertedEnumerator.MoveNext();
                    }
                }
            }
        }
        private List<PUTimeEndTrackable> putimeEnd;
        public List<PUTimeEndTrackable> PUTimeEnd
        {
            get { return putimeEnd; }
        }
        private List<OrderTrackable> orders;
        public List<OrderTrackable> Orders
        {
            get { return orders; }
        }
        private List<BatchTrackable> batches;
        public List<BatchTrackable> Batches
        {
            get { return batches; }
        }
        private List<DowntimeOccasionTrackable> stops;
        public List<DowntimeOccasionTrackable> Stops
        {
            get { return stops; }
        }
        private List<CalendarHistoryTrackable> calendar;
        public List<CalendarHistoryTrackable> Calendar
        {
            get { return calendar; }
        }
    }
}
