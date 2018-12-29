using System;
using Production.Abstract;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class DowntimeOccasionTrackable : DowntimeOccasion, ITrackable, IDeserializableDbRecord
    {
        private readonly Track track;
        private readonly INullMask mask;
        public DowntimeOccasionTrackable(
             Track track,
             INullMask mask,
             int prodPlaceId,
             int id,
             DateTime start,
             DateTime? end = null,
             int? lossType = null
            ) : base(
                prodPlaceId, id, start, end, lossType)
        {
            this.track = track;
            this.mask = mask;
        }
        public DowntimeOccasionTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: mask,
            track: new TrackBinary(data, pos),
            id: SoxxaBitConverter.ToInt32(data, pos + 9),
            prodPlaceId: SoxxaBitConverter.ToInt32(data, pos + 13),
            start: SoxxaBitConverter.ToDateTime(data, pos + 17),
            end: SoxxaBitConverter.ToDateTimeNullable(data, pos + 25, mask, 0),
            lossType: SoxxaBitConverter.ToInt32Nullable(data, pos + 25 + (mask[0] ? 0 : 8), mask, 1))
        {
        }
        public Track Track
        {
            get { return track; }
        }
        public INullMask Nulls
        {
            get { return mask; }
        }
        public int Size
        {
            get
            {
                return 25 + (mask[0] ? 0 : 8) + (mask[1] ? 0 : 4);
            }
        }
    }
}
