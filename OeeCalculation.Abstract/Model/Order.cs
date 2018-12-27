using System.Collections.Generic;
using System.Linq;
using System;
namespace Production.Abstract.Model
{
    public class Order : IOperatorStationEntity, ICompletedProductionPeriod
    {
        private readonly int operatorStationId;
        private readonly int id;
        private DateTime startTime;
        private DateTime? endTime;
        private readonly IEnumerable<OrderBatch> batches;
        private readonly IEnumerable<PUTimeEnd> puEnd;
        private readonly IEnumerable<PUTimeStart> puStart;
        private readonly IEnumerable<PUTimeScrapped> puScrapped;
        private float amountPerUnit;
        private float amountPerPulseStart;
        private float goalCycleTime;
        public Order() { }
        public Order(
            int operatorStationId,
            int id,
            DateTime startTime,
            DateTime? endTime,
            IEnumerable<OrderBatch> batches,
            IEnumerable<PUTimeEnd> puEnd,
            IEnumerable<PUTimeStart> puStart,
            IEnumerable<PUTimeScrapped> puScrapped,
            float amountPerUnit,
            float amountPerPulseStart,
            float goalCycleTime)
        {
            this.operatorStationId = operatorStationId;
            this.id = id;
            this.startTime = startTime;
            this.endTime = endTime;
            this.batches = batches;
            this.puEnd = puEnd;
            this.puStart = puStart;
            this.puScrapped = puScrapped;
            this.amountPerUnit = amountPerUnit;
            this.amountPerPulseStart = amountPerPulseStart;
            this.goalCycleTime = goalCycleTime;
        }
        public int OperatorStation_Id { get { return operatorStationId; } }
        public int Order_Id { get { return id; } }
        public DateTime StartTime { get { return startTime; } set { startTime = value; } }
        public DateTime? EndTime { get { return endTime; } set { endTime = value; } }
        public IEnumerable<OrderBatch> Batches { get { return batches; } }
        public IEnumerable<PUTimeEnd> PUTimeEnds { get { return puEnd; } }
        public IEnumerable<PUTimeStart> PuTimeStarts { get { return puStart; } }
        public IEnumerable<PUTimeScrapped> PuTimeScrappeds { get { return puScrapped; } }
        public float AmountPerUnit { get { return amountPerUnit; } }
        public float AmountPerPulseStart { get { return amountPerPulseStart; } }
        public float GoalCycleTime { get { return goalCycleTime; } }
        public DateTime? End
        {
            get { return EndTime; }
        }
    }
}
