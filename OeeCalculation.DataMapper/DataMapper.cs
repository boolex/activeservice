using System.Linq;
using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.DataMapper
{
    using OeeCalculation.Computing.ProductionEvents;
    using OeeCalculation.Computing.Production;
    using System;

    public class DataMapper : IComputingEventDataMapper
    {
        private readonly IOsProduction production;
        private readonly ILogger logger;
        private readonly IEnumerable<ITrackable> dbChanges;
        public DataMapper(
            IDatabaseChangeSet changeSet,
            IOsProduction production,
            ILogger logger = null) : this(changeSet.Events, production, logger)
        {
        }
        public DataMapper(
            IEnumerable<ITrackable> dbChanges,
            IOsProduction production,
            ILogger logger = null
            )
        {
            this.dbChanges = dbChanges;
            this.production = production;
            this.logger = new NullableLogger(logger);
        }
        public IEnumerable<IComputingEvent> Events
        {
            get
            {
                return dbChanges.Select(Get);
            }
        }
        private IComputingEvent Get(ITrackable trackable)
        {
            IComputingEvent e;
            if (trackable is CalendarHistory calendar)
            {
                e = CalendarEvent(calendar, trackable);
            }
            else if (trackable is DowntimeOccasion downtime)
            {
                e = DowntimeEvent(downtime, trackable);
            }
            else if (trackable is OrderBatch batch)
            {
                e = BatchEvent(batch, trackable);
            }
            else if (trackable is Order order)
            {
                e = OrderEvent(order, trackable);
            }
            else if (trackable is PUTimeEnd producedUnit)
            {
                e = GetProducedUnitEvent(producedUnit, trackable);
            }
            else if (trackable is PUTimeStart startedUnit)
            {
                e = GetStartedUnitEvent(startedUnit, trackable);
            }
            else if (trackable is PUTimeScrapped scrappedUnit)
            {
                e = GetScrappedUnitEvent(scrappedUnit, trackable);
            }
            else
            {
                e = new UnknownProductionEvent(trackable);
            }
            logger.LogDebug(string.Format(
                 "Db record {0} -> production event {1}", trackable, e
            ));
            return e;
        }
        private CalendarHistory recentCalendar;
        private CalendarHistory RecentCalendar
        {
            get { return recentCalendar ?? (recentCalendar = production.Recent.RecentCalendar); }
            set { recentCalendar = value; }
        }
        private Order _recentOrder;
        private Order RecentOrder
        {
            get { return _recentOrder ?? (_recentOrder = production.Recent.RecentOrder); }
            set { _recentOrder = value; }
        }
        private OrderBatch _recentBatch;
        private OrderBatch RecentBatch
        {
            get { return _recentBatch ?? (_recentBatch = production.Recent.RecentBatch); }
            set { _recentBatch = value; }
        }
        private Dictionary<int, DowntimeOccasion> recentDowntime;
        private Dictionary<int, DowntimeOccasion> RecentDowntime
        {
            get
            {
                return recentDowntime ??
                    (production.Prodplaces.ToDictionary(x => x.Machine.ProdPlaceId.Value, y => y.Recent.RecentDowntime));
            }
        }
        private DowntimeOccasion RecentDowntimeOccasionOn(int prodplaceId)
        {
            if (RecentDowntime.ContainsKey(prodplaceId))
            {
                return RecentDowntime[prodplaceId];
            }
            return null;
        }
        private void UpdateRecentDowntimeOccasion(DowntimeOccasion downtime, DowntimeOccasion recent, int prodplaceId)
        {
            if (recent == null || downtime.StartTime >= recent.End)
            {
                if (!RecentDowntime.ContainsKey(prodplaceId))
                {
                    RecentDowntime[prodplaceId] = downtime;
                }
                else
                {
                    RecentDowntime.Add(prodplaceId, downtime);
                }
            }
        }
        private IComputingEvent GetDowntimeEvent(ITrackable trackable, DowntimeOccasion downtime, DowntimeOccasion recent)
        {
            IComputingEvent result;
            if (trackable.Track.Type == TrackingType.Inserted
              && downtime.EndTime == null
              && (recent == null || downtime.StartTime >= recent.End))
            {
                result = new DowntimeStarted(downtime);
            }
            else if (trackable.Track.Type == TrackingType.Inserted
                && downtime.EndTime.HasValue
                && (recent == null || downtime.StartTime >= recent.End))
            {
                result = new EntireDowntimeCompleted(downtime);
            }
            else if (trackable.Track.Type == TrackingType.Updated
                && !recent.End.HasValue
                && (recent != null && downtime.DTOccasion_Id == recent.DTOccasion_Id))
            {
                result = new ActiveDowntimeCompleted(downtime);
            }
            else
            {
                result = new UnknownProductionEvent(trackable);
            }
            return result;
        }
        private IComputingEvent DowntimeEvent(DowntimeOccasion downtime, ITrackable trackable)
        {
            int prodplaceId = trackable.Machine.ProdPlaceId.Value;
            var recent = RecentDowntimeOccasionOn(prodplaceId);
            IComputingEvent result = GetDowntimeEvent(trackable, downtime, recent);
            UpdateRecentDowntimeOccasion(downtime, recent, prodplaceId);
            return result;
        }
        private IComputingEvent BatchEvent(OrderBatch batch, ITrackable trackable)
        {
            IComputingEvent result = GetBatchEvent(batch, RecentBatch, RecentOrder, trackable);
            UpdateRecentBatchEvent(batch, RecentBatch);
            return result;
        }
        private void UpdateRecentBatchEvent(OrderBatch batch, OrderBatch recent)
        {
            if (recent == null || batch.Id == recent.Id || batch.Start >= recent.End)
            {
                RecentBatch = batch;
            }
        }
        private IComputingEvent GetBatchEvent(OrderBatch batch, OrderBatch recent, Order recentOrder, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
                && !batch.End.HasValue
                && (recent == null || batch.StartTime >= recent.End))
            {
                if (recentOrder != null && recentOrder.Order_Id == recent.OrderId)
                {
                    return new OrderStarted(recentOrder, recent);
                }
            }
            else if (trackable.Track.Type == TrackingType.Updated
                && batch.End.HasValue
                && batch.Id == RecentBatch.Id)
            {
                if (recentOrder != null
                    && recentOrder.Order_Id == recent.OrderId
                    && !recentOrder.Active)
                {
                    return new ActiveOrderCompleted(recentOrder, recent);
                }
            }
            else if (trackable.Track.Type == TrackingType.Inserted
                && batch.End.HasValue
                && batch.StartTime >= recent.End)
            {
                return new EntireBatchCompleted(batch);
            }
            return new UnknownProductionEvent(trackable);
        }
        private IComputingEvent OrderEvent(Order order, ITrackable trackable)
        {
            IComputingEvent result = GetOrderEvent(order, RecentOrder, RecentBatch, trackable);
            UpdateRecentOrder(order, RecentOrder, trackable);
            return result;
        }
        private void UpdateRecentOrder(Order order, Order recent, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
             && !order.End.HasValue
             && order.Active)
            {
                RecentOrder = order;
            }
            else if (trackable.Track.Type == TrackingType.Updated
               && order.Active)
            {
                RecentOrder = order;
            }
            else if (trackable.Track.Type == TrackingType.Updated
               && !order.Active)
            {
                RecentOrder = order;
            }
        }
        private IComputingEvent GetOrderEvent(Order order, Order recent, OrderBatch recentBatch, ITrackable trackable)
        {
            //new inserted
            if (trackable.Track.Type == TrackingType.Inserted
                && !order.End.HasValue
                && order.Active)
            {
                if (recentBatch != null && recentBatch.OrderId == order.Order_Id)
                {
                    return new OrderStarted(order, recentBatch);
                }
            }
            //updated
            else if (trackable.Track.Type == TrackingType.Updated
                && order.Active)
            {
                if (recentBatch != null && recentBatch.OrderId == order.Order_Id)
                {
                    return new OrderStarted(order, recentBatch);
                }
            }
            else if (trackable.Track.Type == TrackingType.Updated
                && !order.Active)
            {
                if (recentBatch != null
                    && recentBatch.OrderId == order.Order_Id
                    && recentBatch.End.HasValue)
                {
                    return new ActiveOrderCompleted(order, recentBatch);
                }
            }
            return new UnknownProductionEvent(trackable);
        }
        private IComputingEvent CalendarEvent(CalendarHistory calendar, ITrackable trackable)
        {
            IComputingEvent result = GetCalendarEvent(calendar, RecentCalendar, trackable);
            UpdateRecentCalendar(calendar, RecentCalendar, result);
            return result;
        }
        private void UpdateRecentCalendar(CalendarHistory calendar, CalendarHistory recent, IComputingEvent e)
        {
            if (e.GetType() != typeof(UnknownProductionEvent))
            {
                RecentCalendar = calendar;
            }
        }
        private IComputingEvent GetCalendarEvent(CalendarHistory calendar, CalendarHistory recent, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
                && calendar.Calendar == 4
                && (recent == null || calendar.PeriodStartTime >= recent.ChangeDate)
                && calendar.ChangeType > 0)
            {
                return new ShiftStarted(calendar);
            }
            if (trackable.Track.Type == TrackingType.Updated
                && calendar.Calendar == 1
                && calendar.Id == recent.Id)
            {
                return new ShiftCompleted(calendar);
            }
            return new UnknownProductionEvent(trackable);
        }
        private IComputingEvent GetProducedUnitEvent(PUTimeEnd unit, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
                && RecentOrder != null
                && unit.OrderId == RecentOrder.Order_Id
                && RecentOrder.Active
                && RecentBatch != null
                && !RecentBatch.End.HasValue)
            {
                return new UnitProduced(unit, RecentOrder, RecentBatch);
            }
            return new UnknownProductionEvent(trackable);
        }
        private IComputingEvent GetStartedUnitEvent(PUTimeStart unit, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
                && RecentOrder != null
                && unit.OrderId == RecentOrder.Order_Id
                && RecentOrder.Active
                && RecentBatch != null
                && !RecentBatch.End.HasValue)
            {
                return new UnitStarted(unit, RecentOrder, RecentBatch);
            }
            return new UnknownProductionEvent(trackable);
        }
        private IComputingEvent GetScrappedUnitEvent(PUTimeScrapped unit, ITrackable trackable)
        {
            if (trackable.Track.Type == TrackingType.Inserted
                && RecentOrder != null
                && unit.OrderId == RecentOrder.Order_Id
                && RecentOrder.Active
                && RecentBatch != null
                && !RecentBatch.End.HasValue)
            {
                return new UnitScrapped(unit, RecentOrder, RecentBatch);
            }
            return new UnknownProductionEvent(trackable);
        }
    }
}
