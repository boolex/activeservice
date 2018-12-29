using System;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class PUTimeEndTrackable : PUTimeEnd, ITrackable
    {
        private readonly Track track;
        private readonly INullMask mask;
        protected PUTimeEndTrackable()
            : base()
        {
        }
        public PUTimeEndTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: NullMask.Empty,
            track: new Track(data, pos),
            amount: SoxxaBitConverter.ToSingle(data, pos + 9),
            orderId: SoxxaBitConverter.ToInt32(data, pos + 17),
            putime: SoxxaBitConverter.ToDateTime(data, pos + 21))
        { }

        public PUTimeEndTrackable(
            Track track,
            INullMask mask,
            int orderId,
            float amount,
            DateTime putime
            )
            : base(orderId, amount, putime)
        {
            this.track = track;
            this.mask = mask;
        }
        public INullMask Nulls { get { return mask; } }
        public Track Track { get { return track; } }
        public int Size { get { return 29; } }
    }
}
