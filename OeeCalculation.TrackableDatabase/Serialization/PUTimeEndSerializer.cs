using System;
using System.CodeDom;
using System.Collections.Generic;
using OeeCalculation.TrackableDatabase.Model;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Serialization
{
    public class PUTimeEndSerializer
    {
        public static List<PUTimeEndTrackable> From(byte[] data, int startPos, int length)
        {
            var result = new List<PUTimeEndTrackable>();
            for (var i = 0; i < length; i++)
            {
                result.Add(GetPUTimeEnd(startPos + i * 29, data));
            }
            return result;
        }

        private static PUTimeEndTrackable GetPUTimeEnd(int pos, byte[] data)
        {
            return new PUTimeEndTrackable(
                 track: new Track(
                 date: AxxosBitConverter.ToDateTime(data,pos+0),
                 type: (TrackingType)AxxosBitConverter.ToChar(data, pos + 8)
                 ),
            amount: AxxosBitConverter.ToSingle(data, pos + 9),
             orderId: AxxosBitConverter.ToInt32(data, pos + 17),
             putime: AxxosBitConverter.ToDateTime(data, pos + 21)
                );
        }
    }
}
