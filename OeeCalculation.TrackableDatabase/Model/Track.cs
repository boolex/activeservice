using System;

namespace OeeCalculation.TrackableDatabase.Model
{
    public enum TrackingType
    {
        Deleted = 0,
        Inserted = 1,
        Updated = 2
    }
    public class Track
    {
        private readonly TrackingType type;
        private readonly DateTime date;
        public Track(DateTime date, TrackingType type)
        {
            this.type = type;
            this.date = date;
        }
        public TrackingType Type{get { return type; }}
        public DateTime Date{get { return date; }}
    }
}
