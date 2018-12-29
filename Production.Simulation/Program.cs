using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Production.Simulation
{
    public class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Parse("2018-01-01 01:00:00");
            var end = DateTime.Parse("2018-01-01 01:10:00");
            var now = start;
            var p = new ProductionHistory(new ProductionSetting(), start, end);
            var timeSpeedRatio = 1;

            var sqlComm = new SqlCreateCommand("Server=axkhm01\\sql2008r2;Initial Catalog=OEECoreTest;User ID=sa;Password=0Soxxa0");
            var history = p.History.ToList();
            var historyEnum = history.GetEnumerator();
            var containsEvents = historyEnum.MoveNext();
            TimeSpan baseStep = TimeSpan.FromSeconds(1);
            var cursorTop = Console.CursorTop;
            while (containsEvents && now <= end)
            {
                while (containsEvents && historyEnum.Current.On <= now)
                {
                    Console.WriteLine(historyEnum.Current);
                    new SqlProductionEvent(historyEnum.Current, sqlComm).Create();
                    containsEvents = historyEnum.MoveNext();
                    cursorTop = Console.CursorTop;
                }
                //if (timeSpeedRatio != 0)
                //{
                    Thread.Sleep((int)(baseStep.TotalMilliseconds / timeSpeedRatio));
                //}
                now = now.Add(baseStep);
                Console.SetCursorPosition(40, cursorTop);
                Console.Write(now);
                Console.SetCursorPosition(0, cursorTop);
            }
        }
    }
    public class ProductionHistory
    {
        private readonly DateTime from;
        private readonly DateTime to;
        private readonly ProductionSetting setting;
        public ProductionHistory(ProductionSetting setting, DateTime from, DateTime to)
        {
            this.setting = setting;
            this.from = from;
            this.to = to;
            events = OperatorStations.ToDictionary(x => x.Setting, y => y.NextEvent.GetEnumerator());
            foreach (var e in events)
            {
                if (!e.Value.MoveNext())
                {
                    finished.Add(e.Key);
                }
            }
            finished = new List<OperatorStationSetting>();
        }
        private List<OsProductionHistory> operatorStations;
        private List<OsProductionHistory> OperatorStations
        {
            get
            {
                return operatorStations ?? (operatorStations = setting.OperatorStationSettings.Select(x => new OsProductionHistory(x, from, to)).ToList());
            }
        }
        Dictionary<OperatorStationSetting, IEnumerator<ProductionEvent>> events;
        List<OperatorStationSetting> finished;
        public IEnumerable<ProductionEvent> History
        {
            get { return NextEvent; }
        }
        private IEnumerable<ProductionEvent> NextEvent
        {
            get
            {
                while (finished.Count < OperatorStations.Count)
                {
                    var on = events.Where(x => !finished.Contains(x.Key)).Min(x => x.Value.Current.On);
                    var e = events.Where(x => !finished.Contains(x.Key)).First(x => x.Value.Current.On == on);
                    var prodEvent = e.Value.Current;
                    if (!e.Value.MoveNext())
                    {
                        finished.Add(e.Key);
                    }
                    yield return prodEvent;
                }
                yield break;
            }
        }
    }
    public class OsProductionHistory
    {
        private readonly OperatorStationSetting setting;
        private readonly DateTime from;
        private readonly DateTime to;
        public OsProductionHistory(OperatorStationSetting setting, DateTime from, DateTime to)
        {
            this.setting = setting;
            this.from = from.Add(setting.StartShift);
            this.to = to;
        }
        public IEnumerable<ProductionEvent> History
        {
            get { return NextEvent; }
        }
        public OperatorStationSetting Setting { get { return setting; } }
        private ProductionEvent orderbatch;
        private ProductionEvent unit;
        private ProductionEvent shift;
        private ProductionEvent stop;

        public IEnumerable<ProductionEvent> NextEvent
        {
            get
            {
                if (orderbatch == null)
                {
                    NextOrderBatchEvent();
                    NextUnitEvent();
                    NextShiftEvent();
                    NextStopEvent();
                }
                var e = orderbatch;
                while (e.On <= to || !e.Start)
                {
                    if (orderbatch.On < unit.On && orderbatch.On < shift.On && orderbatch.On < stop.On)
                    {
                        yield return orderbatch;
                        NextOrderBatchEvent();
                        e = orderbatch;
                    }
                    else if (unit.On < shift.On && unit.On < stop.On)
                    {
                        yield return unit;
                        NextUnitEvent();
                        e = unit;
                    }
                    else if (shift.On < stop.On)
                    {
                        yield return shift;
                        NextShiftEvent();
                        e = shift;
                    }
                    else
                    {
                        yield return stop;
                        NextStopEvent();
                        e = stop;
                    }
                }
                if (e.On > to && e.Start)
                {
                    yield break;
                }
            }
        }
        private void NextOrderBatchEvent()
        {
            if (orderbatch == null)
            {
                orderbatch = new ProductionEvent(
                    osId: setting.Id,
                   id: 1,
                     type: ProductionEventType.Order,
                   on: from,
                   start: true);
            }
            else if (orderbatch.Start)
            {
                orderbatch = new ProductionEvent(
                   osId: setting.Id,
                   id: orderbatch.Id,
                   type: ProductionEventType.Order,
                   on: orderbatch.On.Add(setting.Order.BatchDuration),
                   start: false);
            }
            else
            {
                orderbatch = new ProductionEvent(
                    osId: setting.Id,
                    id: orderbatch.Id + 1,
                    type: ProductionEventType.Order,
                    on: orderbatch.On.Add(TimeSpan.FromSeconds(86400 / setting.Order.BatchPerDay) - setting.Order.BatchDuration),
                    start: true
                    );
            }
        }
        private void NextUnitEvent()
        {
            if (unit == null)
            {
                unit = new ProductionEvent(
                     osId: setting.Id,
                   id: orderbatch.Id,
                     type: ProductionEventType.Unit,
                   on: from,
                   start: true);
            }
            else if (unit.Start)
            {
                unit = new ProductionEvent(
                     osId: setting.Id,
                   id: orderbatch.Id,
                   type: ProductionEventType.Unit,
                   on: unit.On.Add(setting.Order.UnitDuration),
                   start: false);
            }
            else
            {
                unit = new ProductionEvent(
                     osId: setting.Id,
                    id: orderbatch.Id,
                   type: ProductionEventType.Unit,
                   on: unit.On.Add(TimeSpan.FromSeconds(60 / setting.Order.UnitsPerMinute) - setting.Order.UnitDuration),
                    start: true
                    );
            }
        }
        private void NextShiftEvent()
        {
            if (shift == null)
            {
                shift = new ProductionEvent(
                     osId: setting.Id,
                   id: 1,
                     type: ProductionEventType.Shift,
                   on: from,
                   start: true);
            }
            else if (shift.Start)
            {
                shift = new ProductionEvent(
                     osId: setting.Id,
                   id: shift.Id,
                   type: ProductionEventType.Shift,
                   on: shift.On.Add(setting.Calendar.ShiftDuration),
                   start: false);
            }
            else
            {
                shift = new ProductionEvent(
                     osId: setting.Id,
                    id: shift.Id + 1,
                   type: ProductionEventType.Shift,
                   on: shift.On.Add(TimeSpan.FromSeconds(86400 / setting.Calendar.ShiftPerDay) - setting.Calendar.ShiftDuration),
                    start: true
                    );
            }
        }
        private void NextStopEvent()
        {
            if (stop == null)
            {
                stop = new ProductionEvent(
                     osId: setting.Id,
                   id: 1,
                     type: ProductionEventType.Stop,
                   on: from,
                   start: true);
            }
            else if (stop.Start)
            {
                stop = new ProductionEvent(
                     osId: setting.Id,
                   id: stop.Id,
                   type: ProductionEventType.Stop,
                   on: stop.On.Add(setting.ProdPlace.StopDuration),
                   start: false);
            }
            else
            {
                stop = new ProductionEvent(
                     osId: setting.Id,
                    id: stop.Id + 1,
                   type: ProductionEventType.Stop,
                   on: stop.On.Add(TimeSpan.FromSeconds(86400 / setting.ProdPlace.StopsPerDay) - setting.ProdPlace.StopDuration),
                    start: true
                    );
            }
        }
    }
    public class ProductionEvent
    {
        private readonly int osId;
        private readonly int id;//order batch id, calendar history id, dto id, order id for unit
        private readonly ProductionEventType type;
        private readonly bool start;
        private readonly DateTime on;
        private readonly int attribute;//order Id, team id, loss type, amount for unit
        public ProductionEvent(
            int osId,
            int id,
            ProductionEventType type,
            DateTime on,
            bool start)
        {
            this.osId = osId;
            this.id = id;
            this.type = type;
            this.on = on;
            this.start = start;
        }
        public int OperatorStationId { get { return osId; } }
        public int Id { get { return id; } }
        public DateTime On { get { return on; } }
        public Boolean Start { get { return start; } }
        public ProductionEventType Type { get { return type; } }
        public override string ToString()
        {
            return string.Format("OS={0} {1} {2} of {3}", osId, on.ToString("yyyy-MM-dd HH:mm:ss"), start ? "start" : "end", type);
        }
    }
    public enum ProductionEventType
    {
        Order,
        Stop,
        Shift,
        Unit
    };
    public class ProductionSetting
    {
        public List<OperatorStationSetting> OperatorStationSettings = new List<OperatorStationSetting>()
        {
            new OperatorStationSetting(osId:1,shift:TimeSpan.FromSeconds(1), unitsPerMinute:1.05f),
            new OperatorStationSetting(osId:2,shift:TimeSpan.FromSeconds(11), unitsPerMinute:1.05f),
            new OperatorStationSetting(osId:3,shift:TimeSpan.FromSeconds(5), unitsPerMinute:1.05f),
            new OperatorStationSetting(osId:4,shift:TimeSpan.FromSeconds(21), unitsPerMinute:1.05f),
            new OperatorStationSetting(osId:5,shift:TimeSpan.FromSeconds(12), unitsPerMinute:1.05f)
        };
    }
    public class OperatorStationSetting
    {
        public CalendarSetting Calendar = new CalendarSetting();
        public OrderSetting Order;
        public ProdPlaceSetting ProdPlace = new ProdPlaceSetting();
        private readonly int osId;
        public int Id { get { return osId; } }
        private readonly TimeSpan shift;
        public OperatorStationSetting(
            int osId,
            TimeSpan shift,
            float unitsPerMinute = 1)
        {
            this.osId = osId;
            this.shift = shift;
            Order = new OrderSetting(unitsPerMinute: unitsPerMinute);
        }
        public TimeSpan StartShift { get { return shift; } }
    }
    public class CalendarSetting
    {
        public TimeSpan ShiftDuration = TimeSpan.FromHours(8);
        public float ShiftPerDay = 2;
    }
    public class OrderSetting
    {
        public float unitsPerMinute;
        public OrderSetting(float unitsPerMinute)
        {
            this.unitsPerMinute = unitsPerMinute;
        }
        public TimeSpan BatchDuration = TimeSpan.FromSeconds(20);
        public float BatchPerDay = 24 * 30;
        public int BatchCountOnOrder = 1;
        public float UnitsPerMinute
        {
            get { return unitsPerMinute; }
        }
        public TimeSpan UnitDuration = TimeSpan.FromSeconds(2);
    }
    public class ProdPlaceSetting
    {
        public TimeSpan StopDuration = TimeSpan.FromMinutes(1);
        public float StopsPerDay = 300;
    }
}
