using Production.Abstract;
namespace OeeCalculation.Computing
{
    using System.Collections.Generic;
    using OeeCalculation.Computing.Production;
    using OeeCalculation.Computing.ProductionEvents;
    public interface IComputingEventDataMapperFactory
    {
        IComputingEventDataMapper NewMapper(IEnumerable<ITrackable> dbChanges, IOsProduction production);
    }
}
