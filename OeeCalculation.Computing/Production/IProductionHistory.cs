using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Production
{
    public interface IProductionHistory
    {
        CalendarHistory RecentCalendar { get; }
        Order RecentOrder { get; }
        OrderBatch RecentBatch { get; }
        DowntimeOccasion RecentDowntime { get; }
    }
    public class ProductionHistory : IProductionHistory
    {
        private readonly CalendarHistory calendar;
        private readonly Order order;
        private readonly OrderBatch batch;
        private readonly DowntimeOccasion downtime;
        public ProductionHistory(
            CalendarHistory calendar,
            Order order,
            OrderBatch batch,
            DowntimeOccasion downtime)
        {
            this.calendar = calendar;
            this.order = order;
            this.batch = batch;
            this.downtime = downtime;
        }
        public CalendarHistory RecentCalendar { get { return calendar; } }
        public Order RecentOrder { get { return order; } }
        public OrderBatch RecentBatch { get { return batch; } }
        public DowntimeOccasion RecentDowntime { get { return downtime; } }
    }
    public class SiteProductionHistory
    {
        private readonly ISiteProduction production;
        public SiteProductionHistory(ISiteProduction production)
        {
            this.production = production;
        }
        private OperatorStationProduction OperatorStation(IMachine machine)
        {
            return (OperatorStationProduction)production.OperatorStation(machine);
        }
        private ProdPlaceProduction Prodplace(IMachine machine)
        {
            return (ProdPlaceProduction)production.Prodplace(machine);
        }
    }
}
