using System;
using System.Linq;
using Production.Abstract.Model;
using OeeCalculation.TrackableDatabase.Model;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OeeCalculation.TrackableDatabase;
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
            var mapper = new DataMapper(changeSet);
            Assert.IsTrue(mapper.Events.Count() > 0);
        }
    }
}
