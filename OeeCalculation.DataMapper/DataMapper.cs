using System;
using System.Linq;
using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
using Production.Abstract.ProductionEvent;

namespace OeeCalculation.DataMapper
{
    public class DataMapper : IDataMapper
    {
        private readonly IDatabaseChangeSet set;
        public DataMapper(IDatabaseChangeSet set)
        {
            this.set = set;
        }
        public IEnumerable<IProductionEvent> Events
        {
            get
            {
                return set.Events.Select(Get);
            }
        }
        private IProductionEvent Get(ITrackable trackable)
        {
            if(trackable is CalendarHistory)
            {
                return GetCalendarEvent((CalendarHistory)trackable, trackable);
            }
            throw new System.NotImplementedException();
        }
        private IProductionEvent GetCalendarEvent(CalendarHistory calendar, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted && calendar.Calendar == 4)
            {
                return new ShiftStarted(calendar);
            }
            if (trackable.Track.Type == TrackingType.Updated && calendar.Calendar == 1)
            {
                return new ShiftEnded(calendar);
            }
            throw new NotImplementedException();
        }
    }
}
