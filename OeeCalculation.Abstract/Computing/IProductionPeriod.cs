namespace Production.Abstract
{
    public interface IProductionPeriod
    {
        IProductionStatistics GetStatistics(DateRange range = null);
    }
}
