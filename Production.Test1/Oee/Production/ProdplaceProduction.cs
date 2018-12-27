using System.Collections.Generic;
namespace Production.Test1.Oee
{
    using Production.Abstract.Model;
    using Production.Abstract;
    using Production.Test1.Oee.Calc;
    using Production.Test1.Oee.Calc.Storage;
    public class ProdPlaceProduction
    {
        private readonly DynamicStorage<DowntimeOccasion> downtimes;
        private readonly PpProductionPeriodCalculator ppCalculator;
        private readonly OperatorStationProduction osProd;
        public ProdPlaceProduction(
            OperatorStationProduction osProd,
            OsProductionPeriodCalculator osCalculator,
            List<DowntimeOccasion> downtimeContext)
        {
            downtimes = new DynamicStorage<DowntimeOccasion>(downtimeContext);
            ppCalculator = new PpProductionPeriodCalculator(osProd, osCalculator, this);
        }
        public DynamicStorage<DowntimeOccasion> Downtimes { get { return downtimes; } }
        public void StartDowntime(DowntimeOccasion dto)
        {
            Downtimes.Start(dto);
        }
        public void StopDowntime(DowntimeOccasion dto)
        {
            Downtimes.End(dto);
        }
        public IProductionStatistics GetStatistics(DateRange range)
        {
            return ppCalculator.Get(range);
        }
        public IProductionStatistics GetActiveOrderStatistics(DateRange range)
        {
            return ppCalculator.GetForActiveOrder(range);
        }

        public IProductionStatistics GetActiveShiftStatistics(DateRange range)
        {
            return ppCalculator.GetForActiveShift(range);
        }
    }
}
