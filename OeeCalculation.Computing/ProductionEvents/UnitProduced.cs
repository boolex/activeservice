using Production.Abstract.Model;
namespace OeeCalculation.Computing.ProductionEvents
{
    using OeeCalculation.Computing.Production;
    public class UnitProduced : IComputingEvent
    {
        private readonly PUTimeEnd unit;
        private readonly Order order;
        private readonly OrderBatch batch;
        public UnitProduced(PUTimeEnd unit, Order order, OrderBatch batch)
        {
            this.unit = unit;
            this.order = order;
            this.batch = batch;
        }
        public void Apply(OperatorStationProduction production)
        {
            throw new System.NotImplementedException();
        }
    }
}
