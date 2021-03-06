﻿using System.Linq;
using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Production
{
    using OeeCalculation.Computing.Calculator;
    using OeeCalculation.Computing.ProductionEvents;
    using OeeCalculation.Computing.Storage;
    /// <summary>
    /// Model of OperatorStation in real-time
    /// 
    /// </summary>
    public class OperatorStationProduction : IOsProduction
    {
        private readonly int id;
        private readonly OsProductionPeriodCalculator calculator;
        private readonly ShiftStorage shiftCalendar;
        private readonly List<ProdPlaceProduction> prodplaces;
        private readonly DynamicStorage<Order> orders = new DynamicStorage<Order>();
        private readonly DynamicStorage<OrderBatch> orderBatches;
        private readonly ProductionOrderBatchStorage productionOrderBatch;
        private readonly ProducedUnitStorage units;
        public OperatorStationProduction(
            int id,
            List<Shift> shiftContext = null,
            List<Order> ordersContext = null)
        {
            this.id = id;
            shiftCalendar = new ShiftStorage(shiftContext);
            orderBatches = new DynamicStorage<OrderBatch>(ordersContext.SelectMany(x => x.Batches).ToList());
            orders = new DynamicStorage<Order>(ordersContext);
            productionOrderBatch = new ProductionOrderBatchStorage(this.shiftCalendar, orderBatches);
            calculator = new OsProductionPeriodCalculator(this);
            prodplaces = new List<ProdPlaceProduction>();
            units = new ProducedUnitStorage(calendar: shiftCalendar);
        }
        public int Id { get { return id; } }
        public void HandleEvents(IEnumerable<IComputingEvent> events)
        {
            foreach (var e in events)
            {
                e.Apply(this);
            }
        }
        public List<ProdPlaceProduction> ProdPlaces { get { return prodplaces; } }
        public ProducedUnitStorage Units { get { return units; } }
        public ShiftStorage Shifts { get { return shiftCalendar; } }
        public IDynamicStorage<Order> Orders { get { return orders; } }
        public IDynamicStorage<OrderBatch> Orderbatches { get { return orderBatches; } }
        public ProductionOrderBatchStorage ProductionOrderBatch { get { return productionOrderBatch; } }

        public ProdPlaceProduction AddProdPlace(ProdPlace prodplace, List<DowntimeOccasion> downtimeContext = null)
        {
            var production = new ProdPlaceProduction(prodplace.Id, this, calculator, downtimeContext);
            prodplaces.Add(production);
            return production;
        }

        public void StartOrderBatch(OrderBatch order)
        {
            orders.Start(orders.All.Single(x => x.Order_Id == order.OrderId));
            orderBatches.Start(order);
        }

        public void Update(IComputingEvent e)
        {
            throw new System.NotImplementedException();
        }

        //public void StopOrderBatch(OrderBatch order) { orderBatches.End(order); }
        //public void StartShift(Shift shift) { shiftCalendar.Start(shift); }
        //public void StopShift(Shift shift) { shiftCalendar.End(shift); }
        //public void StartUnit(PUTimeStart unit) { }
        //public void EndUnit(PUTimeEnd unit) { units.EndUnit(unit); }
        //public void ScrapUnit(PUTimeScrapped unit) { }

        private CalendarHistory recentCalendar;
        public CalendarHistory RecentCalendar { get { return recentCalendar; } }
        private Order recentOrder;
        public Order RecentOrder { get { return recentOrder; } }
        private OrderBatch recentBatch;
        public OrderBatch RecentBatch { get { return recentBatch; } }


        public IMachine Machine
        {
            get
            {
                return new Machine(Id, null);
            }
        }
        public IEnumerable<IProdplaceProduction> Prodplaces
        {
            get
            {
                return ProdPlaces;
            }
        }
        public IProductionHistory Recent
        {
            get
            {
                return new ProductionHistory(
                    RecentCalendar,
                    RecentOrder,
                    RecentBatch,
                    downtime: (ProdPlaces != null && Prodplaces.Any()) ? Prodplaces.First().Recent.RecentDowntime : null
                );
            }
        }
    }
}
