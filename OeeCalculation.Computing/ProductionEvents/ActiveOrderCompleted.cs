using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using OeeCalculation.Computing.Production;
    public class ActiveOrderCompleted : IComputingEvent
    {
        private readonly Order order;
        private readonly OrderBatch batch;
        public ActiveOrderCompleted(Order order, OrderBatch batch)
        {
            this.order = order;
            this.batch = batch;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
