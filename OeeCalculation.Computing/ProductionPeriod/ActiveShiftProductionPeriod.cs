using OeeCalculation.Computing.Production;
using Production.Abstract;
namespace OeeCalculation.Computing.ProductionPeriod
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
