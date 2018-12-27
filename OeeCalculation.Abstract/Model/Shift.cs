using Production.Abstract;
using System;
namespace Production.Abstract.Model
{
    public class Shift : ICompletedProductionPeriod
    {
        private readonly int id;
        private readonly DateTime start;
        private readonly DateTime? end;
        public Shift(
            int id,
            DateTime start,
            DateTime? end = null
            )
        {
            this.id = id;
            this.start = start;
            this.end = end;
        }
        public int Id { get { return id; } }
        public DateTime Start { get { return start; } }
        public DateTime? End { get { return end; } }
       
        public DateTime StartTime
        {
            get { return Start; }
        }        
    }
}
