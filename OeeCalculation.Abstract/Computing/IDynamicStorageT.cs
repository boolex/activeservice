using System;
using System.Collections.Generic;
namespace Production.Abstract
{
    public delegate void CompletedPeriodDelegate<T>(T period);
    public delegate void StartedPeriodDelegate<T>(T period);
    public interface IDynamicStorage<T> where T : ICompletedProductionPeriod
    {
        List<T> History { get; }
        T Active { get; }
        IEnumerable<T> All { get; }
        void Start(T p);
        void End(T p);       
        event CompletedPeriodDelegate<T> Completed;
        event StartedPeriodDelegate<T> Started;
    }
}
