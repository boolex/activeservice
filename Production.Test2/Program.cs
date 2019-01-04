using System.Collections.Generic;
using System.Linq;
using OeeCalculation.Computing;
using OeeCalculation.Computing.Production;
using OeeCalculation.Computing.ProductionEvents;
using OeeCalculation.DataMapper;
using OeeCalculation.TrackableDatabase;
using Production.Abstract;
namespace Production.Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var now = DateTime.Now;
            //Created OS Production
            var osProduction = new OeeCalculation.Computing.Production.OperatorStationProduction();
            var prodplaceProduction = osProduction.AddProdPlace(
               new ProdPlace(1, 1, null),
               downtimeContext: new List<DowntimeOccasion>()
                   {
                        new DowntimeOccasion(1,1,now.AddHours(-4).AddMinutes(-45),now.AddHours(-3), 2)
                   }
           );
            var productionPeriod5Min = new SlidingProductionPeriod(back: TimeSpan.FromMinutes(5), prodPlace: prodplaceProduction);
            //DataBase records
            IDatabaseChangeSet changeSet = new DatabaseChangeSet(null);
            //Converted to Production events
            IComputingEventDataMapper mapper = new DataMapper(changeSet, prodplaceProduction);
            var events = mapper.Events;
            //Applied on OperatorStation production     
            osProduction.HandleEvents(events);
            //Affect OEE calculation
            var nowRange = new DateRange(null, now);
            Console.WriteLine("5 Minutes back");
            Console.WriteLine(FormattedMessage(productionPeriod5Min.GetStatistics(nowRange)));
            */

            IChangeSetSource changeSetSource = null;
            IComputingEventDataMapperFactory mapperFactory = new DataMapperFactory();
            ISiteProduction production = new SiteProduction(changeSetSource, mapperFactory);
            production.Listen();


        }

        public class DataMapperFactory : IComputingEventDataMapperFactory
        {
            public DataMapperFactory()
            {
            }
            public IComputingEventDataMapper NewMapper(IEnumerable<ITrackable> trackables, IOsProduction production)
            {
                return new DataMapper(trackables, production);
            }
        }



        private static string FormattedMessage(IProductionStatistics st)
        {
            if (st == null)
            {
                return "Empty";
            }
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                //{ "Availability", st.Availability.ToString() },
                //{ "Performance", st.Performance.ToString() },
                //{ "Quality", st.Quality.ToString() },
                { "OEE", st.OEE.ToString() }
                //,{"EstProdTime",st.Metrics.Estimated.ToString("hh':'mm':'ss") },
                //{"Scrapped Time",st.Metrics.ScrapTime.ToString("hh':'mm':'ss") },
                //{"Planned Time",st.Metrics.Time.Planned.ToString("hh':'mm':'ss") },
                //{"Produced Amount",st.Metrics.Amount.InProduction.EndedAmount.ToString() },
                //{"Scrapped Amount",st.Metrics.Amount.InProduction.ScrappedAmount.ToString() }
            };
            return string.Join("\n", values.Select(x => string.Format("{0,15}\t{1:5}", x.Value, x.Key)));
        }
    }
}
