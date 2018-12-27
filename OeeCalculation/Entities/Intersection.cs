using System;
namespace Production
{
    public class Intersection
    {
        private readonly int prodplaceId;
        private readonly int? calendarId;
        private readonly int? orderId;
        private readonly int? dtoId;
        private readonly DateTime from;
        private readonly DateTime to;
        public Intersection(int prodplaceId, int? calendarId, int? orderId, int? dtoId, DateTime from, DateTime to)
        {
            this.prodplaceId = prodplaceId;
            this.calendarId = calendarId;
            this.orderId = orderId;
            this.dtoId = dtoId;
            this.from = from;
            this.to = to;
        }
        public int ProdplaceId { get { return prodplaceId; } }
        public int? CalendarId { get { return calendarId; } }
        public int? OrderId { get { return orderId; } }
        public int? DtoId { get { return dtoId; } }
        public DateTime From { get { return from; } }
        public DateTime To { get { return to; } }
        public bool Valid
        {
            get
            {
                return (CalendarId.HasValue ? 1 : 0) + (OrderId.HasValue ? 1 : 0) + (DtoId.HasValue ? 1 : 0) > 1 && to > from;
            }
        }
    }
}
