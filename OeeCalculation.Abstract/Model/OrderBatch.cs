using System;
namespace Production.Abstract.Model
{
    public class OrderBatch : IOperatorStationEntity, ICompletedProductionPeriod
    {
        private readonly int operatorStationId;
        private readonly int orderId;
        private readonly int id;
        private readonly DateTime start;
        private readonly DateTime? end;
        public OrderBatch(
            int orderId,
            int id,
            DateTime start,
            DateTime? end)
        {
            this.orderId = orderId;
            this.id = id;
            this.start = start;
            this.end = end;
        }
        public OrderBatch(int operatorStationId, OrderBatch ob)
        {
            this.operatorStationId = operatorStationId;
            this.orderId = ob.OrderId;
            this.id = ob.Id;
            this.start = ob.Start;
            this.end = ob.End;
        }
        public int OrderId { get { return orderId; } }
        public int Id { get { return id; } }
        public DateTime Start { get { return start; } }
        public DateTime? End { get { return end; } }
        public int OperatorStation_Id
        {
            get { return operatorStationId; }
        }


        public DateTime StartTime
        {
            get { return Start; }
        }
    }
}
