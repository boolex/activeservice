using System;
namespace Production.Abstract
{
    public interface ILossStatistics
    {
        TimeSpan Planned { get; }
        TimeSpan Availability { get; }
        TimeSpan Speed { get; }
        TimeSpan Rework { get; }
        TimeSpan System { get; }
    }
}
