using System.Collections.Generic;
using System.Linq;
namespace Production
{
    public class IntersectionContext
    {
        private readonly IEnumerable<ProductionEvent> events;
        public IntersectionContext(IEnumerable<ProductionEvent> events)
        {
            this.events = events;
        }
        private IEnumerable<Intersection> intersections;
        public IEnumerable<Intersection> Intersections
        {
            get { return intersections ?? (intersections = getIntersections()); }
        }
        private IEnumerable<Intersection> getIntersections()
        {
            var inters = new List<Intersection>();
            //put order and shift before 0 event

            var firstShiftEvent = events.FirstOrDefault(x => x.Type == ProductionEventType.ShiftEnd || x.Type == ProductionEventType.ShiftStart);
            var firstBatchEvent = events.FirstOrDefault(x => x.Type == ProductionEventType.OrderEnd || x.Type == ProductionEventType.OrderStart);
            var firstStopEvent = events.FirstOrDefault(x => x.Type == ProductionEventType.DtoEnd || x.Type == ProductionEventType.DtoStart);
            int? currentCalendarId = null, currentOrderBatchId = null, currentDtoId = null;
            if (firstShiftEvent != null && firstShiftEvent.Type == ProductionEventType.ShiftEnd)
            {
                currentCalendarId = firstShiftEvent.Id;
            }
            if (firstBatchEvent != null && firstBatchEvent.Type == ProductionEventType.OrderEnd)
            {
                currentOrderBatchId = firstBatchEvent.Id;
            }
            if (firstStopEvent != null && firstStopEvent.Type == ProductionEventType.DtoEnd)
            {
                currentDtoId = firstStopEvent.Id;
            }
            ProductionEvent prevProdEvent = firstShiftEvent;
            if (firstBatchEvent != null && (prevProdEvent == null || prevProdEvent.Time > firstBatchEvent.Time))
            {
                prevProdEvent = firstBatchEvent;
            }
            if (firstStopEvent != null && (prevProdEvent == null || prevProdEvent.Time > firstStopEvent.Time))
            {
                prevProdEvent = firstStopEvent;
            }
            foreach (var productionEvent in events)
            {
                switch (productionEvent.Type)
                {
                    case ProductionEventType.ShiftStart:
                        currentCalendarId = productionEvent.Id;
                        break;
                    case ProductionEventType.ShiftEnd:
                        currentCalendarId = null;
                        break;
                    case ProductionEventType.OrderStart:
                        currentOrderBatchId = productionEvent.Id;
                        break;
                    case ProductionEventType.OrderEnd:
                        currentOrderBatchId = null;
                        break;
                    case ProductionEventType.DtoStart:
                        currentDtoId = productionEvent.Id;
                        break;
                    case ProductionEventType.DtoEnd:
                        currentDtoId = null;
                        break;
                    default:
                        break;
                }
                var intersection = new Intersection(
                    prodplaceId: productionEvent.ProdPlace,
                    calendarId: currentCalendarId,
                    orderId: currentOrderBatchId,
                    dtoId: currentDtoId,
                    from: prevProdEvent.Time,
                    to: productionEvent.Time
                    );
                if (intersection.Valid)
                {
                    inters.Add(intersection);
                }
                prevProdEvent = productionEvent;
            }
            return inters;
        }
    }
}
