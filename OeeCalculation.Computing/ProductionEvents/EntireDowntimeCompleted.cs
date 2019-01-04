using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using OeeCalculation.Computing.Production;
    public class EntireDowntimeCompleted : IComputingEvent
    {
        private readonly DowntimeOccasion downtime;
        public EntireDowntimeCompleted(DowntimeOccasion downtime)
        {
            this.downtime = downtime;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
