using System;
namespace Production.Abstract
{
    public interface IProductionBaseAttributes
    {
        IProductionTimeStatistics Time { get; }
        IOverallAmountStatistics Amount { get; }
    }
    public interface IOverallAmountStatistics
    {
        IAmountStatistics Total { get; }
        IAmountStatistics InProduction { get; }
    }
    public interface IProductionTimeMetrics : IProductionBaseAttributes
    {
        TimeSpan Estimated { get; }
        TimeSpan SpeedCalcLoss { get; }
        TimeSpan ScrapTime { get; }
    }
    public interface IProductionStatistics
    {
        IProductionTimeMetrics Metrics { get; }
        float? Availability { get; }
        float? Quality { get; }
        float? Performance { get; }
        float? OEE { get; }
    }
    public interface IAmountStatistics
    {
        float StartedAmount { get; }
        float EndedAmount { get; }
        float ScrappedAmount { get; }
    }
    public interface IOsProductionStatistics
    {
        TimeSpan ScheduledTime { get; }
    }
    public interface IOrderProductionStatistics
    {
        TimeSpan OrderScheduledTime { get; }
        TimeSpan OrderNotPlanProdLoss { get; }
        TimeSpan OrderAvailabilityLoss { get; }
        float? OrderAvailability { get; }
        float? OEE { get; }
    }
}
