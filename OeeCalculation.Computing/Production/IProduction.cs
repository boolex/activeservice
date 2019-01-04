using System.Collections.Generic;
using Production.Abstract;
namespace OeeCalculation.Computing.Production
{
    using OeeCalculation.Computing.ProductionEvents;
    public interface IProduction
    {
        void Update(IComputingEvent e);
        IProductionHistory Recent { get; }
    }
    public interface IProdplaceProduction : IProduction
    {
        IMachine Machine { get; }
    }
    public interface IOsProduction : IProduction
    {
        IMachine Machine { get; }
        IEnumerable<IProdplaceProduction> Prodplaces { get; }
    }
    public delegate void ProducitonUpdated(IEnumerable<IComputingEvent> events);
    public interface IProductionEventSource
    {
        event ProducitonUpdated Updated;
    }
    public interface ISiteProduction 
    {        
        IOsProduction OperatorStation(IMachine machine);
        IProdplaceProduction Prodplace(IMachine machine);
        /// <summary>
        /// Wait environment events and update its state
        /// </summary>
        void Listen();
    }
}
