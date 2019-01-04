using System;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.Computing.Calculator
{
    using OeeCalculation.Computing.Production;
    using OeeCalculation.Computing.Statistics;
    using OeeCalculation.Computing.Storage;
    public class OsProductionPeriodCalculator
    {
        private readonly OperatorStationProduction production;
        private readonly ProductionOrderBatchStorage productionOrder;
        public OsProductionPeriodCalculator(OperatorStationProduction production)
        {
            this.production = production;
            this.productionOrder = new ProductionOrderBatchStorage(production.Shifts, production.Orderbatches);
        }
        public ProductionOrderBatchStorage ProductionOrder
        {
            get { return productionOrder; }
        }
        public IOsProductionStatistics GetStatistics(DateRange range)
        {
            return new OsProductionStatistics(
                scheduledTime: production.Shifts.GetScheduledTime(range)
            );
        }
        public DateTime? GetCurrentShiftStart()
        {
            return production.Shifts.CurrentShiftStartTime;
        }

        public Shift GetCurrentShift()
        {
            return production.Shifts.CurrentShift;
        }
    }
}

