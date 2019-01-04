using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Production
{
    using OeeCalculation.Computing.Calculator;
    using OeeCalculation.Computing.ProductionEvents;
    using OeeCalculation.Computing.Storage;
    public class ProdPlaceProduction : IProductionHistory, IProdplaceProduction
    {
        private readonly int id;
        private readonly DynamicStorage<DowntimeOccasion> downtimes;
        private readonly PpProductionPeriodCalculator ppCalculator;
        private readonly OperatorStationProduction osProd;
        public ProdPlaceProduction(
            int id,
            OperatorStationProduction osProd,
            OsProductionPeriodCalculator osCalculator,
            List<DowntimeOccasion> downtimeContext)
        {
            this.id = id;
            downtimes = new DynamicStorage<DowntimeOccasion>(downtimeContext);
            ppCalculator = new PpProductionPeriodCalculator(osProd, osCalculator, this);
        }
        public int Id { get { return id; } }
        public DynamicStorage<DowntimeOccasion> Downtimes { get { return downtimes; } }

        public CalendarHistory RecentCalendar { get { return osProd.RecentCalendar; } }

        public Order RecentOrder { get { return osProd.RecentOrder; } }

        public OrderBatch RecentBatch { get { return osProd.RecentBatch; } }

        private DowntimeOccasion recentDowntime;
        public DowntimeOccasion RecentDowntime
        {
            get
            {
                return recentDowntime;
            }
            private set
            {
                recentDowntime = value;
            }
        }

        public IMachine Machine
        {
            get
            {
                return new Machine(osProd.Id, Id);
            }
        }

        public IProductionHistory Recent
        {
            get
            {
                return new ProductionHistory(osProd.RecentCalendar, osProd.RecentOrder, osProd.RecentBatch, RecentDowntime);
            }
        }

        public void StartDowntime(DowntimeOccasion dto)
        {
            RecentDowntime = dto;
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

        public void Update(IComputingEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
