using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using OeeCalculation.Computing.Production;
    public class ShiftCompleted : IComputingEvent
    {
        private readonly CalendarHistory calendar;
        public ShiftCompleted(CalendarHistory calendar)
        {
            this.calendar = calendar;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
