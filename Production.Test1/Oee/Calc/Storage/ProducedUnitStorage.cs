using System;
using System.Collections.Generic;
using System.Linq;
using Production.Test1.Oee.Statistics;
using Production.Abstract;
using Production.Abstract.Model;
namespace Production.Test1.Oee.Calc.Storage
{
    public class ProducedUnitStorage
    {
        private Dictionary<int, SortedList<DateTime, float>> total;
        public Dictionary<int, SortedList<DateTime, float>> Total { get { return total; } }
        private Dictionary<int, SortedList<DateTime, float>> production;
        public Dictionary<int, SortedList<DateTime, float>> Production { get { return production; } }
        private readonly ShiftStorage calendar;
        public ProducedUnitStorage(ShiftStorage calendar)
        {
            this.calendar = calendar;
            this.total = new Dictionary<int, SortedList<DateTime, float>>();
            this.production = new Dictionary<int, SortedList<DateTime, float>>();
            
        }
        public IOverallAmountStatistics GetAmount(OrderBatch batch, DateRange range)
        {
            return new OverallAmountStatistics(
                total: new AmountStatistics(
                    started: 1,
                    ended: GetTotalEndedAmount(batch, range),
                    scrapped: 1
                    ),
                 production: new AmountStatistics(
                    started: 1,
                    ended: GetProductionEndedAmount(batch, range),
                    scrapped: 1
                    )
                );
        }
        private float GetTotalEndedAmount(OrderBatch batch, DateRange range)
        {
            if (!Production.ContainsKey(batch.OrderId))
            {
                return 0;
            }
            return Production[batch.OrderId]
                .Where(x => x.Key >= batch.Start && (x.Key <= batch.End || !batch.End.HasValue))
                .Sum(x => x.Value);
        }
        private float GetProductionEndedAmount(OrderBatch batch, DateRange range)
        {
            if (!Total.ContainsKey(batch.OrderId))
            {
                return 0;
            }
            return Total[batch.OrderId]
                .Where(x => x.Key >= batch.Start && (x.Key <= batch.End || !batch.End.HasValue))
                .Sum(x => x.Value);
        }
        public void EndUnit(PUTimeEnd end)
        {
            if (!total.ContainsKey(end.OrderId))
            {
                total.Add(end.OrderId, new SortedList<DateTime, float>());
            }
            if (total[end.OrderId].ContainsKey(end.PUTime))
            {
                total[end.OrderId][end.PUTime] = end.Amount;
            }
            else
            {
                total[end.OrderId].Add(end.PUTime, end.Amount);
            }


            if (calendar.GetShift(end.PUTime) != null)
            {
                if (!production.ContainsKey(end.OrderId))
                {
                    production.Add(end.OrderId, new SortedList<DateTime, float>());
                }
                if (production[end.OrderId].ContainsKey(end.PUTime))
                {
                    production[end.OrderId][end.PUTime] = end.Amount;
                }
                else
                {
                    production[end.OrderId].Add(end.PUTime, end.Amount);
                }
            }
        }
    }
}
