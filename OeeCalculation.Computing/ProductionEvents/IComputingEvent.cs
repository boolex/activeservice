using OeeCalculation.Computing.Production;
using Production.Abstract;

namespace OeeCalculation.Computing.ProductionEvents
{    
    public interface IComputingEvent
    {
        void Apply(OperatorStationProduction production);
    }
}
