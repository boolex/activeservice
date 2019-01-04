using System;
using System.Collections.Generic;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class OrderTrackable : Order, ITrackable, IDeserializableDbRecord
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
            float goalCycleTime,
            bool active
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
             goalCycleTime,
             active)
        {
            this.track = track;
            this.mask = mask;
        }
        public OrderTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: mask,
            track: new TrackBinary(data, pos),
            operatorStationId: SoxxaBitConverter.ToInt32(data, pos + 9),
            id: SoxxaBitConverter.ToInt32(data, pos + 13),
            startTime: SoxxaBitConverter.ToDateTime(data, pos + 17),
            amountPerUnit: SoxxaBitConverter.ToSingle(data, pos + 25),
            amountPerPulseStart: SoxxaBitConverter.ToSingle(data, pos + 33),
            goalCycleTime: SoxxaBitConverter.ToSingle(data, pos + 41),
            endTime: SoxxaBitConverter.ToDateTimeNullable(data, pos + 49, mask, 0),
            batches: null,
            puEnd: null,
            puStart: null,
            puScrapped: null,
            active: false)
        { }
        public Track Track { get { return track; } }
        public int Size { get { return 49 + (mask[0] ? 0 : 8); } }
        public INullMask Nulls { get { return mask; } }

        public IMachine Machine { get { return new Machine(OperatorStation_Id); } }
    }
}
