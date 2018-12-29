using System;
using System.Linq;
using Production.Test1.Oee;
using System.Threading;
using System.Collections.Generic;
using Production.Test1.Oee.ProductionPeriod;
using Production.Abstract;
using Production.Abstract.Model;
using OeeCalculation.DatabaseContext;
using Production.Test1.MsSql;
using OeeCalculation.TrackableDatabase;
namespace Production.Test1
{
    /// <summary>
    /// TODO:
    /// *1.Need Production Downtime
    ///     a)up to date    -   track DynamicStorage for Shift and DowntimeChanged?
    ///     b)no recalculation - add items when event finished
    /// *2.
    ///     Production statistics for 
    ///         a.Active orderbatch and order
    ///         b.Active shift
    ///         c.Ongoing period with static start date (Today, CurrentWeek)
    ///         d.Lasting period like last 5min, 1h, 3h, 7d
    /// 
    /// *3.
    ///     LoadDatabaseContext Context for longest period
    /// *4
    ///     New order started : how to load old batches ?(load its statistics, on change => reload whole orderbatch period from db)
    /// *5. Extract Model to separate project to decrease coupling between DB and other projects
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var cs = @"Server=axkhm01\sql2008r2;Initial Catalog=OEECoreTest;User Id=sa;Password=0Soxxa0";
            //var db = new SoxxaDatabase(
            //    new DbRequest(connectionString: cs),
            //    DateTime.Parse("2018-01-01")
            //   /* ,new SqlServerListener(cs)*/);
            //db.Start();
            //db.Listen();
            //Console.ReadLine();

            var db = new SoxxaTrackableDatabase(cs);
            db.Track();

           // Thread.Sleep(10000);
            return;
            DateTime now = DateTime.Now;
            var shiftHistory = new List<Shift>();
            for (var i = 0; i < 1000; i++)
            {
                shiftHistory.Add(new Shift(i % 2 + 1, now.AddHours(-9).AddHours((0 - i) * 8), now.AddHours(-1).AddHours((0 - i) * 8)));
            }
            var orders = new List<Order>();

            var osProduction = new OperatorStationProduction(
                shiftContext: shiftHistory,
                ordersContext: new List<Order>
                {
                    new Order(
                        operatorStationId: 1,
                        id: 1,
                        startTime: now.AddMinutes(-300),
                        endTime: null,
                        batches: new List<OrderBatch>{
                             new OrderBatch(1, 2, now.AddMinutes(-250), now.AddMinutes(-210)),
                            new OrderBatch(1,4, now.AddMinutes(-110), now.AddMinutes(-90)),
                        },
                        puEnd:new List<PUTimeEnd>{
                            new PUTimeEnd(1, 1000,  now.AddMinutes(-250)),
                            new PUTimeEnd(1, 1000,  now.AddMinutes(-249)),
                            new PUTimeEnd(1, 1000,  now.AddMinutes(-248)),
                        },
                        puScrapped:null,
                        puStart:null,
                        amountPerUnit:1,
                        amountPerPulseStart:1,
                        goalCycleTime:1),
                     new Order(
                        operatorStationId: 1,
                        id: 2,
                        startTime: now.AddMinutes(-200),
                        endTime: null,
                        batches: new List<OrderBatch>{
                             new OrderBatch(2,1, now.AddMinutes(-300), now.AddMinutes(-260)),
                             new OrderBatch(2,3, now.AddMinutes(-170), now.AddMinutes(-130)),
                             new OrderBatch(2,5, now.AddMinutes(-85), now.AddMinutes(-40))
                        },
                        puEnd:new List<PUTimeEnd>(){
                            new PUTimeEnd(2, 10,  now.AddMinutes(-300))
                        },
                        puScrapped:null,
                        puStart:null,
                        amountPerUnit:1,
                        amountPerPulseStart:1,
                        goalCycleTime:1)
                }
            );
            osProduction.EndUnit(new PUTimeEnd(2, 4500, now.AddMinutes(-50)));

            osProduction.StartShift(new Shift(id: 1, start: now.AddMinutes(-30)));
            osProduction.StartOrderBatch(new OrderBatch(1, 9, now.AddMinutes(-15), null));
            var prodplaceProduction = osProduction.AddProdPlace(
                new ProdPlace(1, 1, null),
                downtimeContext: new List<DowntimeOccasion>()
                    {
                        new DowntimeOccasion(1,1,now.AddHours(-4).AddMinutes(-45),now.AddHours(-3), 2)
                    }
            );
            prodplaceProduction.StartDowntime(new DowntimeOccasion(1, 2, now.AddMinutes(-2), null, 2));

            var productionPeriod5Min = new SlidingProductionPeriod(back: TimeSpan.FromMinutes(5), prodPlace: prodplaceProduction);
            var productionPeriod100Min = new SlidingProductionPeriod(back: TimeSpan.FromMinutes(100), prodPlace: prodplaceProduction);
            var productionPeriod1Day = new SlidingProductionPeriod(back: TimeSpan.FromDays(1), prodPlace: prodplaceProduction);
            var productionPeriodActiveOrder = new ActiveOrderProductionPeriod(production: prodplaceProduction);
            var productionPeriodActiveShift = new ActiveShiftProductionPeriod(production: prodplaceProduction);

            var productionPeriod3h = new SlidingProductionPeriod(back: TimeSpan.FromHours(3), prodPlace: prodplaceProduction);
            var productionPeriod7Day = new SlidingProductionPeriod(back: TimeSpan.FromDays(7), prodPlace: prodplaceProduction);
            var productionPeriod1Month = new SlidingProductionPeriod(back: TimeSpan.FromDays(30), prodPlace: prodplaceProduction);
            var n = DateTime.Now;
            var nowRange = new DateRange(null, n);
            for (var i = 0; i < 1000; i++)
            {
                osProduction.EndUnit(new PUTimeEnd(1, 1, DateTime.Now));

                Console.Clear();
                Console.WriteLine("Active Shift");
                Console.WriteLine(FormattedMessage(productionPeriodActiveShift.GetStatistics()));
                Console.WriteLine("5 Minutes back");
                Console.WriteLine(FormattedMessage(productionPeriod5Min.GetStatistics(nowRange)));
                Console.WriteLine("100 Minutes back");
                Console.WriteLine(FormattedMessage(productionPeriod100Min.GetStatistics(nowRange)));
                Console.WriteLine("3 Hurs back");
                Console.WriteLine(FormattedMessage(productionPeriod3h.GetStatistics(nowRange)));
                Console.WriteLine("1 Day back");
                Console.WriteLine(FormattedMessage(productionPeriod1Day.GetStatistics(nowRange)));
                Console.WriteLine("7 Days back");
                Console.WriteLine(FormattedMessage(productionPeriod7Day.GetStatistics(nowRange)));
                Console.WriteLine("1 Month back");
                Console.WriteLine(FormattedMessage(productionPeriod1Month.GetStatistics(nowRange)));
                Console.WriteLine("1 Year back");
                Console.WriteLine(FormattedMessage(productionPeriod1Month.GetStatistics(nowRange)));
                Console.WriteLine("Active order");
                Console.WriteLine(FormattedMessage(productionPeriodActiveOrder.GetStatistics()));
                Thread.Sleep(100);
            }

            /*
            for (var i = 0; i < 1000; i++)
            {
                osProduction.EndUnit(new PUTimeEnd(1, 1, DateTime.Now));

                var st = prodplaceProduction.GetStatistics(DateTime.Now);
                Console.WriteLine(FormattedMessage(st));
                Thread.Sleep(100);
                Console.Clear();
            }
             */
            Console.ReadLine();
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
