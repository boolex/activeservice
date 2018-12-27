namespace Production.Abstract
{
    public interface IProductionMetrics
    {
        float? Availability { get; }
        float? Quality { get; }
        float? Performance { get; }
        float? OEE { get; }
    }
}
