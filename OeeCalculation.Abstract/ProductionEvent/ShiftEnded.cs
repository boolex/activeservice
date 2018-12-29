using Production.Abstract.Model;
namespace Production.Abstract.ProductionEvent
{
    public class ShiftEnded : IProductionEvent
    {
        private readonly CalendarHistory calendar;
        public ShiftEnded(CalendarHistory calendar)
        {
            this.calendar = calendar;
        }
    }
}
