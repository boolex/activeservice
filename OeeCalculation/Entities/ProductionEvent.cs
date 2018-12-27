using System;
namespace Production
{
    public class ProductionEvent
    {
        private readonly int prodplace;
        private readonly ProductionEventType type;
        private readonly DateTime time;
        private readonly int id;
        public ProductionEvent(int prodplace, ProductionEventType type, DateTime time, int id)
        {
            this.prodplace = prodplace;
            this.type = type;
            this.time = time;
            this.id = id;
        }
        public int ProdPlace { get { return prodplace; } }
        public ProductionEventType Type { get { return type; } }
        public DateTime Time { get { return time; } }
        public int Id { get { return id; } }
    }
}
