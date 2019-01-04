using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using global::Production.Abstract;
    using OeeCalculation.Computing.Production;
    public class ShiftStarted : IComputingEvent
    {
        private readonly CalendarHistory calendar;
        public ShiftStarted(CalendarHistory calendar)
        {
            this.calendar = calendar;
        }
        public IMachine Target
        {
            get { return new Machine(operatorStationId: calendar.OperatorStation_Id); }
        }
        public void Apply(OperatorStationProduction production)
        {

        }
    }
}
