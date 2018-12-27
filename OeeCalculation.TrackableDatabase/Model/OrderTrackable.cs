using System;
using System.Collections.Generic;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class OrderTrackable : Order, ITrackable
    {
        private readonly Track track;
        private readonly INullMask mask;
        protected OrderTrackable() { }
        public OrderTrackable(
            Track track,
            INullMask mask,
             int operatorStationId,
            int id,
            DateTime startTime,
            DateTime? endTime,
            IEnumerable<OrderBatch> batches,
            IEnumerable<PUTimeEnd> puEnd,
            IEnumerable<PUTimeStart> puStart,
            IEnumerable<PUTimeScrapped> puScrapped,
            float amountPerUnit,
            float amountPerPulseStart,
            float goalCycleTime
            ) : base(
                   operatorStationId,
             id,
             startTime,
             endTime,
             batches,
             puEnd,
             puStart,
             puScrapped,
             amountPerUnit,
             amountPerPulseStart,
             goalCycleTime)
        {
            this.track = track;
            this.mask = mask;
        }
        public OrderTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: mask,
            track: new Track(data, pos),
            operatorStationId: AxxosBitConverter.ToInt32(data, pos + 9),
            id: AxxosBitConverter.ToInt32(data, pos + 13),
            startTime: AxxosBitConverter.ToDateTime(data, pos + 17),
            amountPerUnit: AxxosBitConverter.ToSingle(data, pos + 25),
            amountPerPulseStart: AxxosBitConverter.ToSingle(data, pos + 33),
            goalCycleTime: AxxosBitConverter.ToSingle(data, pos + 41),
            endTime: AxxosBitConverter.ToDateTimeNullable(data, pos + 49, mask, 0),
            batches: null,
            puEnd: null,
            puStart: null,
            puScrapped: null)
        { }
        public Track Track { get { return track; } }
        public int Size { get { return 49 + (mask[0] ? 0 : 8); } }
        public INullMask Nulls { get { return mask; } }
    }
}
