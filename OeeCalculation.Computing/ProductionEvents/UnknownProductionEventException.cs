using System;
using Production.Abstract;
namespace OeeCalculation.Computing.ProductionEvents
{
    public class UnknownProductionEventException : ApplicationException
    {
        private readonly ITrackable trackable;
        public UnknownProductionEventException(ITrackable trackable)
        {
            this.trackable = trackable;
        }
    }
}
