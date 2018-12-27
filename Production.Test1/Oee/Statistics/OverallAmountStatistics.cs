using Production.Abstract;
using System.Collections.Generic;
using System.Linq;
namespace Production.Test1.Oee.Statistics
{
    public class OverallAmountStatistics : IOverallAmountStatistics
    {
        private readonly IAmountStatistics total;
        private readonly IAmountStatistics production;
        public OverallAmountStatistics(IEnumerable<IOverallAmountStatistics> e)
        {
            if (e != null)
            {
                total = new AmountStatistics(
                    started: e.Sum(x => x.Total.StartedAmount),
                    ended: e.Sum(x => x.Total.EndedAmount),
                    scrapped: e.Sum(x => x.Total.ScrappedAmount)
                    );
                production = new AmountStatistics(
                    started: e.Sum(x => x.InProduction.StartedAmount),
                    ended: e.Sum(x => x.InProduction.EndedAmount),
                    scrapped: e.Sum(x => x.InProduction.ScrappedAmount)
                    );
            }
        }
        public OverallAmountStatistics(
            IAmountStatistics total,
            IAmountStatistics production
            )
        {
            this.total = total;
            this.production = production;
        }
        public IAmountStatistics Total
        {
            get { return total; }
        }
        public IAmountStatistics InProduction
        {
            get { return production; }
        }
    }
}
