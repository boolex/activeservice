using System;
using Production.Test1.Oee.Calc;
using Production.Abstract;
using Production.Test1.Oee.Calc.Storage;
using Production.Abstract.Model;
using Production.Test1.Oee.Statistics;
namespace Production.Test1.Oee.Calc
{
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

