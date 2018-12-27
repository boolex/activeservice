using Production.Abstract;
namespace Production.Test1.Oee.ProductionPeriod
{
    public class ActiveOrderProductionPeriod : IProductionPeriod
    {
        private readonly ProdPlaceProduction production;
        public ActiveOrderProductionPeriod(
            ProdPlaceProduction production
            )
        {
            this.production = production;
        }
        public IProductionStatistics GetStatistics(DateRange range = null)
        {
            return production.GetActiveOrderStatistics(range);
        }
    }
}
