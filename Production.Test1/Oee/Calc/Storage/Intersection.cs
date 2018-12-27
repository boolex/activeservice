using Production.Abstract;
using Production.Abstract;
using System;
namespace Production.Test1.Oee.Calc.Storage
{
    public class Intersection<T, F> : ICompletedProductionPeriod
        where T : ICompletedProductionPeriod
        where F : ICompletedProductionPeriod
    {
        private readonly T a;
        private readonly F b;
        public Intersection(T a, F b)
        {
            this.a = a;
            this.b = b;
        }
        public DateTime? End
        {
            get
            {
                if(!a.End.HasValue && !b.End.HasValue)
                {
                    return null;
                }
                if(a.End.HasValue && b.End.HasValue && a.End.Value < b.End.Value)
                {
                    return a.End;
                }
                if (a.End.HasValue && b.End.HasValue && a.End.Value >= b.End.Value)
                {
                    return b.End;
                }
                if (a.End.HasValue)
                {
                    return a.End;
                }
                else
                {
                    return b.End;
                }
            }
        }
        public DateTime StartTime
        {
            get
            {
                return a.StartTime > b.StartTime ? a.StartTime : b.StartTime;
            }
        }
        public T A { get { return a; } }
        public F B { get { return b; } }        
    }
}
