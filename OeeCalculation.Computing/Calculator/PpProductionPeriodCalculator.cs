using System;
using System.Collections.Generic;
using System.Linq;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Calculator
{
    using OeeCalculation.Computing.Production;
    using OeeCalculation.Computing.Statistics;
    using OeeCalculation.Computing.Storage;
    public class PpProductionPeriodCalculator
    {
        private readonly OperatorStationProduction osProd;
        private readonly OsProductionPeriodCalculator osProdCalc;
        private readonly ProdPlaceProduction prod;
        private readonly ProductionDowntimeStorage productionDowntime;
        private readonly ProductionOrderDowntime productionOrderDowntime;
        public PpProductionPeriodCalculator(OperatorStationProduction osProd, OsProductionPeriodCalculator osProdCalc, ProdPlaceProduction prod)
        {
            this.osProdCalc = osProdCalc;
            this.prod = prod;
            this.osProd = osProd;
            this.productionDowntime = new ProductionDowntimeStorage(this.osProd.Shifts, this.prod.Downtimes);
            this.productionOrderDowntime = new ProductionOrderDowntime(osProd.ProductionOrderBatch, prod.Downtimes);
        }
        public IEnumerable<OrderBatchProductionBaseAttributes> GetBatches(DateRange range)
        {
            ICollection<OrderBatchProductionBaseAttributes> active = new List<OrderBatchProductionBaseAttributes>();
            if (osProdCalc.ProductionOrder.Active != null)
            {
                var batch = osProdCalc.ProductionOrder.Active.B;
                active.Add(new OrderBatchProductionBaseAttributes(
                    batch: batch,
                    times: new ProductionTimeStatistics(
                        scheduled: new PeriodDuration(osProdCalc.ProductionOrder.Active).GetDuration(range),
                        loss: productionOrderDowntime.GetForOrderBatch(batch, range)
                    ),
                    amount: osProd.Units.GetAmount(batch, range)
                    ));
            }
            return osProdCalc.ProductionOrder.History
                .Where(x => range.Match(x))
                .GroupBy(x => x.B)
                .ToDictionary(x => x.Key, y => TimeSpan.FromSeconds(y.Sum(t => new PeriodDuration(t).GetDuration(range).TotalSeconds)))
                .Select(x => new OrderBatchProductionBaseAttributes(
                    batch: x.Key,
                    times: new ProductionTimeStatistics(
                        scheduled: x.Value,
                        loss: productionOrderDowntime.GetForOrderBatch(x.Key, range)
                    ),
                    amount: osProd.Units.GetAmount(x.Key, range)
                )).Union(active);
        }
        public IEnumerable<OrderBatchProductionBaseAttributes> GetBatchesOnActiveOrder(DateRange range)
        {
            if (osProdCalc.ProductionOrder.Active != null)
            {
                var activeBatch = osProdCalc.ProductionOrder.Active.B;
                var activeBatchAttr = new OrderBatchProductionBaseAttributes(
                    batch: activeBatch,
                    times: new ProductionTimeStatistics(
                        scheduled: new PeriodDuration(osProdCalc.ProductionOrder.Active).GetDuration(range),
                        loss: productionOrderDowntime.GetForOrderBatch(activeBatch, range)
                        ),
                    amount: osProd.Units.GetAmount(activeBatch, range)
                    );

                return osProdCalc.ProductionOrder.History
                .Where(x => x.B.OrderId == activeBatch.OrderId)
               .GroupBy(x => x.B)
               .ToDictionary(x => x.Key, y => TimeSpan.FromSeconds(y.Sum(t => new PeriodDuration(t).GetDuration(range).TotalSeconds)))
               .Select(x => new OrderBatchProductionBaseAttributes(
                   batch: x.Key,
                   times: new ProductionTimeStatistics(
                       scheduled: x.Value,
                       loss: productionOrderDowntime.GetForOrderBatch(x.Key, range)
                   ),
                   amount: osProd.Units.GetAmount(x.Key, range)
               )).Union(new List<OrderBatchProductionBaseAttributes> { activeBatchAttr });
            }
            return new List<OrderBatchProductionBaseAttributes>();
        }
        public Dictionary<Order, IProductionTimeMetrics> GetOrdersInProduction(DateRange range)
        {
            var d = new Dictionary<Order, IProductionTimeMetrics>();
            var batches = GetBatches(range).GroupBy(x => x.Batch.OrderId).ToDictionary(x => x.Key, y => y);

            foreach (var o in osProd.Orders.All.Where(x => range.Match(x)))
            {
                if (batches.ContainsKey(o.Order_Id))
                {
                    d.Add(
                        key: o,
                        value: new OrderProductionTimeMetrics(
                            order: o,
                            batches: batches[o.Order_Id]
                            )
                        );
                }
            }

            if (osProd.Orders.Active != null)
            {

            }

            return d;
        }

        private DateTime? CurrentShiftStart
        {
            get { return osProdCalc.GetCurrentShiftStart(); }
        }
        public IProductionStatistics GetForActiveShift(DateRange range)
        {
            var shiftStart = CurrentShiftStart;
            if (shiftStart.HasValue)
            {
                return Get(new DateRange(range).Limit(shiftStart.Value, DateTime.Now));
            }
            else
            {
                return null;
            }
        }
        public IProductionStatistics GetForActiveOrder(DateRange range)
        {
            if (osProd.Orders.Active == null)
            {
                return null;
            }
            else
            {
                var batches = GetBatchesOnActiveOrder(range);
                var metrics = new OrderProductionTimeMetrics(
                     order: osProd.Orders.Active,
                     batches: batches
                     );

                float? availability = ((metrics.Time.Planned > TimeSpan.Zero) ?
               ((float)(100 * metrics.Time.Production.TotalSeconds / metrics.Time.Planned.TotalSeconds)) :
               ((float?)null));
                float? quality = ((metrics.Estimated > TimeSpan.Zero) ?
                    ((float)(100 * (metrics.Estimated - metrics.Time.Loss.Rework - metrics.ScrapTime).TotalSeconds / (metrics.Estimated.TotalSeconds))) :
                    ((float?)null));
                float? performance = ((metrics.Estimated > TimeSpan.Zero) ?
                    ((float)(100 * (metrics.Estimated).TotalSeconds / (metrics.Time.Production.TotalSeconds))) :
                    ((float?)null));
                float? oee = (availability == null || quality == null || performance == null) ?
                    ((float?)null) :
                    ((float)availability * performance * quality / 10000);

                return new ProductionStatistics(
                    availability: availability,
                    quality: quality,
                    performance: performance,
                    oee: oee,
                    metrics: new ProductionTimeMetrics(
                        time: metrics.Time,
                        amount: metrics.Amount,
                        estimated: metrics.Estimated,
                        speedCalcLoss: TimeSpan.Zero,
                        scrapTime: metrics.ScrapTime
                        )
                    );
            }
        }
        public IProductionStatistics Get(DateRange range)
        {
            var loss = productionDowntime.GetLossStatistics(range);
            var scheduled = osProd.Shifts.GetScheduledTime(range);
            var orders = GetOrdersInProduction(range);
            var pba = new ProductionBaseAttributes(
               times: new ProductionTimeStatistics(scheduled, loss),
               amount: new OverallAmountStatistics(
                   total: new AmountStatistics(
                       started: orders.Values.Sum(x => x.Amount.Total.StartedAmount),
                       ended: orders.Values.Sum(x => x.Amount.Total.EndedAmount),
                       scrapped: orders.Values.Sum(x => x.Amount.Total.ScrappedAmount)
                       ),
                   production: new AmountStatistics(
                        started: orders.Values.Sum(x => x.Amount.InProduction.StartedAmount),
                        ended: orders.Values.Sum(x => x.Amount.InProduction.EndedAmount),
                        scrapped: orders.Values.Sum(x => x.Amount.InProduction.ScrappedAmount)
                       )
                   )
                );
            var planTime = scheduled - loss.Planned;
            var prodTime = scheduled - loss.Planned - loss.Availability;
            var orderProdTime = TimeSpan.FromSeconds(orders.Values.Sum(o => o.Time.Production.TotalSeconds));
            var estimProdTime = TimeSpan.FromSeconds(orders.Values.Sum(o => o.Estimated.TotalSeconds));

            var scrapTime = TimeSpan.FromSeconds(orders.Values.Sum(o => o.ScrapTime.TotalSeconds));
            var rework = TimeSpan.FromSeconds(orders.Values.Sum(o => o.Time.Loss.Rework.TotalSeconds));

            float? availability = ((planTime > TimeSpan.Zero) ?
                ((float)(100 * prodTime.TotalSeconds / planTime.TotalSeconds)) :
                ((float?)null));
            float? quality = ((estimProdTime > TimeSpan.Zero) ?
                ((float)(100 * (estimProdTime - rework - scrapTime).TotalSeconds / (estimProdTime.TotalSeconds))) :
                ((float?)null));
            float? performance = ((estimProdTime > TimeSpan.Zero) ?
                ((float)(100 * (estimProdTime).TotalSeconds / (prodTime.TotalSeconds))) :
                ((float?)null));
            float? oee = (availability == null || quality == null || performance == null) ?
                ((float?)null) :
                ((float)availability * performance * quality / 10000);

            return new ProductionStatistics(
                availability: availability,
                quality: quality,
                performance: performance,
                oee: oee,
                metrics: new ProductionTimeMetrics(
                    time: pba.Time,
                    amount: pba.Amount,
                    estimated: estimProdTime,
                    speedCalcLoss: TimeSpan.Zero,
                    scrapTime: scrapTime
                    )
                );
        }
    }
}
