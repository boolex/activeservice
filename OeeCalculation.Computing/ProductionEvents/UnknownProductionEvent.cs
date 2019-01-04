using OeeCalculation.Computing.Production;
using Production.Abstract;
namespace OeeCalculation.Computing.ProductionEvents
{
    public class UnknownProductionEvent : IComputingEvent
    {
        private readonly ITrackable trackable;
        public UnknownProductionEvent(ITrackable trackable)
        {
            this.trackable = trackable;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new UnknownProductionEventException(trackable);
        }
    }
}
