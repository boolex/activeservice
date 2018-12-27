using System;
using System.Collections.Generic;
using System.Linq;
using OeeCalculation.TrackableDatabase.Model;
using OeeCalculation.TrackableDatabase.Serialization;

namespace OeeCalculation.TrackableDatabase
{
    public class DatabaseChangeSet
    {
        private readonly byte[] data;
        public DatabaseChangeSet(byte[] data)
        {
            this.data = data;
        }

        public void Load()
        {
            var putimeEndLength = AxxosBitConverter.ToInt32(data, 0);
            putimeEnd = PUTimeEndSerializer.From(data, 4, putimeEndLength);
        }
        private List<PUTimeEndTrackable> putimeEnd;
        public List<PUTimeEndTrackable> PUTimeEnd
        {
            get { return putimeEnd; }
        }
    }
}
