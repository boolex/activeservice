using Production.Abstract;
namespace OeeCalculation.Computing.ProductionPeriod
{
    using OeeCalculation.Computing.Production;
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
