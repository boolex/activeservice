using System;
using System.Linq;
using Production.Abstract.Model;
using OeeCalculation.TrackableDatabase.Model;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OeeCalculation.TrackableDatabase;
using OeeCalculation.Computing.Production;
using OeeCalculation.Computing.ProductionEvents;
using Production.Abstract;

namespace OeeCalculation.DataMapper.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var changeSet = new DatabaseChangeSet(
                calendarsTrackable: new List<CalendarHistoryTrackable>()
                {
                    new CalendarHistoryTrackable(
                        track:new Track(DateTime.Parse("2018-01-01 06:00"),TrackingType.Inserted),
                        mask:NullMask.Empty,
                        operatorStationId:1,
                        calendarHistoryId:1,
                        changeType:1,
                        calendar:4,
                        periodStartTime:DateTime.Parse("2018-01-01 06:00"),
                        changeDate:DateTime.Parse("2018-01-01 06:00")
                    ),
                    new CalendarHistoryTrackable(
                        track:new Track(DateTime.Parse("2018-01-01 07:00"),TrackingType.Updated),
                        mask:NullMask.Empty,
                        operatorStationId:1,
                        calendarHistoryId:1,
                        changeType:1,
                        calendar:1,
                        periodStartTime:DateTime.Parse("2018-01-01 06:00"),
                        changeDate:DateTime.Parse("2018-01-01 07:00")
                    ),
                },
                ordersTrackable: new List<OrderTrackable>()
                {

                },
                batchesTrackable: new List<BatchTrackable>
                {

                },
                stopsTrackable: new List<DowntimeOccasionTrackable>
                {

                },
                unitEndedTrackable: new List<PUTimeEndTrackable>
                {

                }
            );
            var os = new TestOsProduction(1);
            os.AddProdplace(new TestPpProduction(1, os, recentDowntime: null));
            var mapper = new DataMapper(
                changeSet: changeSet,
                production: os
            );
            Assert.IsTrue(mapper.Events.Count() > 0);
        }

        private class TestOsProduction : IOsProduction
        {
            private int id;
            private readonly List<TestPpProduction> prodplaces;
            private readonly CalendarHistory calendar;
            private readonly Order order;
            private readonly OrderBatch batch;
            public TestOsProduction(
                int id,
                CalendarHistory calendar = null,
                Order order = null,
                OrderBatch batch = null)
            {
                this.id = id;
                this.prodplaces = new List<TestPpProduction>();
                this.calendar = calendar;
                this.order = order;
                this.batch = batch;
            }
            public void AddProdplace(TestPpProduction prodplace)
            {
                prodplaces.Add(prodplace);
            }
            public IMachine Machine { get { return new Machine(operatorStationId: id); } }
            public IEnumerable<IProdplaceProduction> Prodplaces
            {
                get { return prodplaces; }
            }
            public IProductionHistory Recent
            {
                get
                {
                    return new ProductionHistory(
                        calendar,
                        order,
                        batch,
                        null);
                }
            }
            public void Update(IComputingEvent e)
            {
                throw new NotImplementedException();
            }
        }
        private class TestPpProduction : IProdplaceProduction
        {
            private readonly int id;
            private readonly TestOsProduction os;
            private readonly DowntimeOccasion recentDowntime;
            public TestPpProduction(
                int id,
                TestOsProduction os,
                DowntimeOccasion recentDowntime)
            {
                this.id = id;
                this.os = os;
                this.recentDowntime = recentDowntime;
            }
            public IMachine Machine
            {
                get
                {
                    return new Machine(
                        operatorStationId: os.Machine.OperatorStationId,
                        prodplaceId: id);
                }
            }

            public IProductionHistory Recent
            {
                get
                {
                    return new ProductionHistory(
                        os.Recent.RecentCalendar,
                        os.Recent.RecentOrder,
                        os.Recent.RecentBatch,
                        recentDowntime);
                }
            }

            void IProduction.Update(IComputingEvent e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
