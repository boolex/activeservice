using System;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class BatchTrackable : OrderBatch, ITrackable, IDeserializableDbRecord
    {
        private readonly INullMask mask;
        private readonly Track track;
        public BatchTrackable(
            Track track,
            INullMask mask,
             int orderId,
            int id,
            DateTime start,
            DateTime? end
            ) : base(orderId, id, start, end)
        {
            this.track = track;
            this.mask = mask;
        }
        public BatchTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: mask,
            track: new TrackBinary(data, pos),
            id: SoxxaBitConverter.ToInt32(data, pos + 9),
            orderId: SoxxaBitConverter.ToInt32(data, pos + 13),
            start: SoxxaBitConverter.ToDateTime(data, pos + 17),
            end: SoxxaBitConverter.ToDateTimeNullable(data, pos + 25, mask, 0))
        { }
        public Track Track
        {
            get { return track; }
        }
        public int Size
        {
            get { return 25 + (mask[0] ? 0 : 8); }
        }
        public INullMask Nulls { get { return mask; } }

        public IMachine Machine { get { return new Machine(orderId: OrderId); } }
    }
}
