using Production.Abstract;
namespace OeeCalculation.Computing.Statistics
{
    public class AmountStatistics : IAmountStatistics
    {
       private readonly float started;
       private readonly float ended;
       private readonly float scrapped;
        public AmountStatistics(
            float started,
            float ended,
            float scrapped
            )
        {
            this.started = started;
            this.ended = ended;
            this.scrapped = scrapped;
        }
        public float StartedAmount
        {
            get { return started; }
        }
        public float EndedAmount
        {
            get { return ended; }
        }
        public float ScrappedAmount
        {
            get { return scrapped; }
        }
    }
}
