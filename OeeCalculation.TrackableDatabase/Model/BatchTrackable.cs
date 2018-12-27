using System;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class BatchTrackable : OrderBatch, ITrackable
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
        public BatchTrackable(INullMask mask, byte[] data, int pos): this(
            mask: mask,
            track: new Track(data, pos),
            id: AxxosBitConverter.ToInt32(data, pos + 9),
            orderId: AxxosBitConverter.ToInt32(data, pos + 13),
            start: AxxosBitConverter.ToDateTime(data, pos + 17),
            end: AxxosBitConverter.ToDateTimeNullable(data, pos + 25, mask, 0))
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
    }
}
