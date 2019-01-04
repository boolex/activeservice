using System;
using Production.Abstract;
namespace OeeCalculation.Computing.Statistics
{
    public class ProductionTimeStatistics : IProductionTimeStatistics
    {
        private readonly TimeSpan scheduled;
        private readonly TimeSpan planned;
        private readonly TimeSpan production;
        private readonly TimeSpan run;
        private readonly ILossStatistics loss;
        public ProductionTimeStatistics(
           TimeSpan scheduled,
           ILossStatistics loss
           ) : this(
               scheduled: scheduled,
               planned: scheduled - loss.Planned,
               production: scheduled - loss.Planned - loss.Availability,
               run: scheduled - loss.Planned - loss.Availability - loss.Speed - loss.Rework - loss.System,
               loss: loss)
        {
        }
        public ProductionTimeStatistics(
            TimeSpan scheduled,
            TimeSpan planned,
            TimeSpan production,
            TimeSpan run,
            ILossStatistics loss
            )
        {
            this.scheduled = scheduled;
            this.planned = planned;
            this.production = production;
            this.run = run;
            this.loss = loss;
        }
        public TimeSpan Scheduled { get { return scheduled; } }
        public TimeSpan Planned { get { return planned; } }
        public TimeSpan Production { get { return production; } }
        public TimeSpan Run { get { return run; } }
        public ILossStatistics Loss { get { return loss; } }
    }
}
