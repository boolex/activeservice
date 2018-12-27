using System;
namespace Production.Abstract
{
    public interface IProductionTimeStatistics
    {
        TimeSpan Scheduled { get; }
        TimeSpan Planned { get; }
        TimeSpan Production { get; }
        TimeSpan Run { get; }
        ILossStatistics Loss { get; }        
    }
}
