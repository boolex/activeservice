using OeeCalculation.Computing.Production;
using Production.Abstract;
using System;
namespace OeeCalculation.Computing.ProductionPeriod
{
    public class SlidingProductionPeriod : IProductionPeriod
    {
        private readonly ProdPlaceProduction prodPlace;
        private readonly TimeSpan back;
        public SlidingProductionPeriod(
            TimeSpan back,
            ProdPlaceProduction prodPlace)
        {
            this.prodPlace = prodPlace;
            this.back = back;
        }
        public IProductionStatistics GetStatistics(DateRange range = null)
        {
            range = range.LimitEnd(DateTime.Now);
            range = range.LimitStart(range.To.Value.Add(-back));
            return prodPlace.GetStatistics(range);
        }
    }
}
