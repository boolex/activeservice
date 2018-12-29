using Production.Abstract.ProductionEvent;
namespace OeeCalculation.Computing.ProductionEvents
{
    public interface IComputingEvent
    {
        void Act();
    }
    public class ShiftStartedComputingEvent
    {
        private readonly ShiftStarted e;
        public ShiftStartedComputingEvent(ShiftStarted e)
        {
            this.e = e;
        }
    }
}
