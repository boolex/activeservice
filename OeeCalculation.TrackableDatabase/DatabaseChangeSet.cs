using System.Collections.Generic;
using OeeCalculation.TrackableDatabase.Model;
using OeeCalculation.TrackableDatabase.Serialization;
namespace OeeCalculation.TrackableDatabase
{
    public class DatabaseChangeSet 
    {
        private readonly byte[] data;
        public DatabaseChangeSet(byte[] data)
        {
            this.data = data;
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
