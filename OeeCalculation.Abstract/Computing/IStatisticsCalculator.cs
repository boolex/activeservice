using System;
namespace Production.Abstract
{
    public interface IStatisticsCalculator
    {
        IProductionBaseAttributes Get(DateRange range);
    }
}
