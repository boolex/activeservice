using OeeCalculation.Computing.Production;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    public class EntireBatchCompleted : IComputingEvent
    {
        private readonly OrderBatch batch;
        public EntireBatchCompleted(OrderBatch batch)
        {
            this.batch = batch;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
