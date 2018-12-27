using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    [Serializable]
    public class PUTimeEndTrackable : PUTimeEnd
    {
        private readonly Track track;

        protected PUTimeEndTrackable()
            : base()
        {
        }

        public PUTimeEndTrackable(
            Track track,
            int orderId,
            float amount,
            DateTime putime
            )
            : base(orderId, amount, putime)
        {
            this.track = track;
        }
        public Track Track { get { return track; } }
    }
}
