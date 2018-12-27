using Production.Abstract;
namespace Production.Test1.Oee.ProductionPeriod
{
    public class ActiveShiftProductionPeriod : IProductionPeriod
    {
        private readonly ProdPlaceProduction production;
        public ActiveShiftProductionPeriod(ProdPlaceProduction production)
        {
            this.production = production;
        }
        public IProductionStatistics GetStatistics(DateRange range = null)
        {
            return production.GetActiveShiftStatistics(range);
        }
    }
}
