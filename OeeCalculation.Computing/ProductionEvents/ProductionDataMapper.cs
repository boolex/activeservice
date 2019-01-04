using System.Collections.Generic;

namespace OeeCalculation.Computing.ProductionEvents
{
    public interface IComputingEventDataMapper
    {
        IEnumerable<IComputingEvent> Events { get; }
    }
}
