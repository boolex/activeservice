using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Production.Test1.MsSql;
using Production.Abstract.Model;
using Production.Abstract;
namespace OeeCalculation.DatabaseContext
{
    public class AxxosDatabase : IDatabaseContext
    {
        //private readonly SqlServerListener listener;
        private readonly DateTime now;
        private readonly IRequest request;
        public AxxosDatabase(IRequest request, DateTime now/*, SqlServerListener listener*/)
        {
            this.now = now;
            this.request = request;
           // this.listener = listener;
        }
        private IEnumerable<OperatorStation> operatorStations;
        IEnumerable<OperatorStation> OperatorStations
        {
            get { return operatorStations; }
        }
        public void Load()
        {
            throw new NotImplementedException();
        }
        public void Start()
        {
            var now = DateTime.Now;
            var startOfDay = now - now.TimeOfDay;
            
            //StartTrackingDatabaseEvents(now);
            LoadDatabaseContext(startOfDay);
            LoadHistoricalCalculations(startOfDay.AddDays(-30), startOfDay);
            LoadCurrentOrderCalculatedBatches(now);
            LoadCurrentShiftParts(now);
            Syncronize(now);
        }
        public void StartTrackingDatabaseEvents(DateTime from)
        {
        }
        public void LoadDatabaseContext(DateTime from)
        {
            var s = request.Proc(
                "[dbo].[spAsl_Std_LoadProductionContext]",
                new[]
                {
                    new SqlParameter("@from", from.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                    new SqlParameter("@now", now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                });
            operatorStations = Build(s);
        }
        public void LoadHistoricalCalculations(DateTime from, DateTime to)
        {
        }
        public void LoadCurrentOrderCalculatedBatches(DateTime now)
        {
        }
        public void LoadCurrentShiftParts(DateTime now)
        {
        }
        public void Syncronize(DateTime now)
        {
        }
        public enum Change
        {
            Inserted,
            Updated,
            Deleted
        }
        public enum Table
        {
            OperatorStation,
            ProdPlace,
            DowntimeOccasion,
            Orders,
            OrderBatch,
            PUTimeEnd,
            PUTimeStart,
            PUTimeScrapped,
            CalendarHistory
        }
        private readonly Dictionary<Table, int> TableOrder = new Dictionary<Table, int>
        {
            {Table.OperatorStation,0},
            {Table.CalendarHistory,1},
            {Table.ProdPlace,2},
            {Table.DowntimeOccasion,3},
            {Table.Orders,4},
            {Table.OrderBatch,5},
            {Table.PUTimeEnd,6},
            {Table.PUTimeStart,7},
            {Table.PUTimeScrapped,8}
        };
        private IEnumerable<OperatorStation> Build(DataSet set)
        {
            var stops = Fetch(
                set.Tables[TableOrder[Table.DowntimeOccasion]],
                r => new DowntimeOccasion(
                    prodPlaceId: GetInt(r, "ProdPlace_Id"),
                    id: GetInt(r, "DTOccasion_Id"),
                    start: GetDateTime(r, "BeginTime"),
                    end: GetDateTimeN(r, "EndTime"),
                    lossType: GetIntN(r, "LossType")));
            var prodplaces = Fetch(
                set.Tables[TableOrder[Table.ProdPlace]],
                r => new ProdPlace(
                    operatorStationId: GetInt(r, "OperatorStation_Id"),
                    id: GetInt(r, "ProdPlace_Id"),
                    stops: stops.Where(x => x.ProdPlace_Id == GetInt(r, "ProdPlace_Id")).ToList()));
            var batches = Fetch(
                set.Tables[TableOrder[Table.OrderBatch]],
                r => new OrderBatch(
                    orderId: GetInt(r, "Order_Id"),
                    id: GetInt(r, "OrderBatch_Id"),
                    start: GetDateTime(r, "StartTime"),
                    end: GetDateTimeN(r, "EndTime")));
            var puTimeEnd = Fetch(
                set.Tables[TableOrder[Table.PUTimeEnd]],
                r => new PUTimeEnd(
                    orderId: GetInt(r, "Order_Id"),
                    amount: GetFloat(r, "Amount"),
                    puTime: GetDateTime(r, "PUTime")));
            var puTimeStart = Fetch(
                set.Tables[TableOrder[Table.PUTimeStart]],
                r => new PUTimeStart(
                    orderId: GetInt(r, "Order_Id"),
                    amount: GetFloat(r, "Amount"),
                    puTime: GetDateTime(r, "PUTime")));
            var puTimeScrapped = Fetch(
                set.Tables[TableOrder[Table.PUTimeScrapped]],
                r => new PUTimeScrapped(
                    orderId: GetInt(r, "Order_Id"),
                    amount: GetFloat(r, "Amount"),
                    puTime: GetDateTime(r, "PUTime")));
            var orders = Fetch(
                set.Tables[TableOrder[Table.Orders]],
                parser: r => new Order(
                    operatorStationId: GetInt(r, "OperatorStation_Id"),
                    id: GetInt(r, "Order_Id"),
                    startTime: GetDateTime(r, "StartTime"),
                    endTime: GetDateTimeN(r, "EndTime"),
                    batches: batches.Where(x => x.OrderId == GetInt(r, "Order_Id")),
                    puEnd: puTimeEnd.Where(x => x.OrderId == GetInt(r, "Order_Id")),
                    puStart: puTimeStart.Where(x => x.OrderId == GetInt(r, "Order_Id")),
                    puScrapped: puTimeScrapped.Where(x => x.OrderId == GetInt(r, "Order_Id")),
                    amountPerPulseStart: 1,
                    amountPerUnit: 1,
                    goalCycleTime: 1));
            var calendarHistory = Fetch(
                set.Tables[TableOrder[Table.CalendarHistory]],
                r => new CalendarHistory(
                    operatorStationId: GetInt(r, "OperatorStation_Id"),
                    calendarHistoryId: GetInt(r, "CalendarHistory_Id"),
                    changeType: GetInt(r, "ChangeType"),
                    calendar: GetInt(r, "Calendar"),
                    periodStartTime: GetDateTime(r, "PeriodStartTime"),
                    changeDate: GetDateTime(r, "ChangeDate")
                    ));
            var oses = Fetch(
                set.Tables[TableOrder[Table.OperatorStation]],
                r => new OperatorStation(
                    id: GetInt(r, "OperatorStation_Id"),
                    calendar: calendarHistory.Where(c => c.OperatorStation_Id == GetInt(r, "OperatorStation_Id")).ToList(),
                    prodplaces: prodplaces.Where(p => p.OperatorStation_Id == GetInt(r, "OperatorStation_Id")).ToList(),
                    orders: orders.Where(o => o.OperatorStation_Id == GetInt(r, "OperatorStation_Id")).ToList()));
            return oses;
        }
        private IEnumerable<T> Fetch<T>(DataTable table, Func<DataRow, T> parser)
        {
            return table.AsEnumerable().Select(x => parser(x));
        }
        private float GetFloat(DataRow row, string field)
        {
            return float.Parse(row[field].ToString());
        }
        private int GetInt(DataRow row, string field)
        {
            return int.Parse(row[field].ToString());
        }
        private int? GetIntN(DataRow row, string field)
        {
            if (row[field] == DBNull.Value)
            {
                return null;
            }
            return GetInt(row, field);
        }
        private DateTime GetDateTime(DataRow row, string field)
        {
            return DateTime.Parse(row[field].ToString());
        }
        private DateTime? GetDateTimeN(DataRow row, string field)
        {
            if (row[field] == DBNull.Value)
                return null;
            return GetDateTime(row, field);
        }
        public void Listen()
        {
           // listenDowntimeOccasion();
           // listenOrders();
            //listenCalendarHistory();
            //listenOrderBatch();
        }
        public delegate void ChangeHandler<T>(T item);
        public event ChangeHandler<Production.Abstract.Model.DowntimeOccasion> OnDowntimeOccasionInserted;
        public event ChangeHandler<DowntimeOccasion> OnDowntimeOccasionUpdated;
        public event ChangeHandler<DowntimeOccasion> OnDowntimeOccasionDeleted;
        public event ChangeHandler<OrderBatch> OnOrderBatchInserted;
        public event ChangeHandler<OrderBatch> OnOrderBatchUpdated;
        public event ChangeHandler<OrderBatch> OnOrderBatchDeleted;
        public event ChangeHandler<Order> OnOrderInserted;
        public event ChangeHandler<Order> OnOrderUpdated;
        public event ChangeHandler<Order> OnOrderDeleted;
        //private void listenDowntimeOccasion()
        //{
        //    var downtimeOccasion = new TableListener<DowntimeOccasion>(listener);
        //    downtimeOccasion.Listen(
        //        inserted: dto =>
        //        {
        //            OperatorStations.SelectMany(x => x.ProdPlaces).Single(p => p.Id == dto.ProdPlace_Id).Stops.Add(dto);
        //            OnDowntimeOccasionInserted(dto);
        //        },
        //        updated: dto =>
        //        {
        //            var prodplace = OperatorStations.SelectMany(x => x.ProdPlaces).Single(p => p.Id == dto.ProdPlace_Id);
        //            var toUpdate = prodplace.Stops.FirstOrDefault(d => d.DTOccasion_Id == dto.DTOccasion_Id);
        //            if (toUpdate != null)
        //            {
        //                prodplace.Stops.Remove(toUpdate);
        //            }
        //            prodplace.Stops.Add(dto);
        //            OnDowntimeOccasionUpdated(dto);
        //        },
        //        deleted: dto =>
        //        {
        //            var prodplace = OperatorStations.SelectMany(x => x.ProdPlaces).Single(p => p.Id == dto.ProdPlace_Id);
        //            var toDelete = prodplace.Stops.FirstOrDefault(x => x.DTOccasion_Id == dto.DTOccasion_Id);
        //            if (toDelete != null)
        //            {
        //                prodplace.Stops.Remove(toDelete);
        //            }
        //            OnDowntimeOccasionDeleted(dto);
        //        }
        //    );
        //}
        //private void listenOrders()
        //{
        //    var ord = new TableListener<Order>(listener, "Orders");
        //    ord.Listen(
        //        inserted: o =>
        //        {
        //            OperatorStations.Single(x => x.OperatorStation_Id == o.OperatorStation_Id).Orders.Add(o);
        //            OnOrderInserted(o);
        //        },
        //        updated: o =>
        //        {
        //            var os = OperatorStations.Single(x => x.OperatorStation_Id == o.OperatorStation_Id);
        //            var item = os.Orders.FirstOrDefault(x => x.Order_Id == o.Order_Id);
        //            if (item != null)
        //            {
        //                os.Orders.Remove(item);
        //            }
        //            os.Orders.Add(o);
        //            OnOrderUpdated(o);
        //        },
        //        deleted: o =>
        //        {
        //            var os = OperatorStations.Single(x => x.OperatorStation_Id == o.OperatorStation_Id);
        //            var item = os.Orders.FirstOrDefault(x => x.Order_Id == o.Order_Id);
        //            if (item != null)
        //            {
        //                os.Orders.Remove(item);
        //            }
        //            OnOrderDeleted(o);
        //        });
        //}

       
    }
}
