using OeeCalculation.Computing.Production;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    public class DowntimeStarted : IComputingEvent
    {
        private readonly DowntimeOccasion downtime;
        public DowntimeStarted(DowntimeOccasion downtime)
        {
            this.downtime = downtime;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
