using System;
namespace Production.Abstract
{
    public interface ICompletedProductionPeriod : IStartedProductionPeriod
    {
        DateTime? End { get; }
    }
}
