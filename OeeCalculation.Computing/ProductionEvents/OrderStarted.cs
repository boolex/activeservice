using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using OeeCalculation.Computing.Production;
    public class OrderStarted : IComputingEvent
    {
        private readonly Order order;
        private readonly OrderBatch batch;
        public OrderStarted(Order order, OrderBatch batch)
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
