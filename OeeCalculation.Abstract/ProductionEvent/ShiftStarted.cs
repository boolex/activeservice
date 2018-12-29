using Production.Abstract.Model;
namespace Production.Abstract.ProductionEvent
{
    public class ShiftStarted : IProductionEvent
    {
        private readonly CalendarHistory calendar;
        public ShiftStarted(CalendarHistory calendar)
        {
            this.calendar = calendar;
        }
    }
}
