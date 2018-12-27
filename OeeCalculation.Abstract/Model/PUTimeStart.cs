using System;
namespace Production.Abstract.Model
{
    public class PUTimeStart
    {
        private readonly int orderId;
        private readonly float amount;
        private readonly DateTime puTime;
        public PUTimeStart(int orderId, float amount, DateTime puTime)
        {
            this.orderId = orderId;
            this.amount = amount;
            this.puTime = puTime;
        }
        public int OrderId { get { return orderId; } }
        public float Amount { get { return amount; } }
        public DateTime PUTime { get { return puTime; } }
    }
}
