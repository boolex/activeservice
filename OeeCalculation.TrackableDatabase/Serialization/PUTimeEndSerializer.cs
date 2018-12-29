using System;
using System.Collections.Generic;
using OeeCalculation.TrackableDatabase.Model;

namespace OeeCalculation.TrackableDatabase.Serialization
{
    public class DbListSerializer<T> where T : class, ITrackable
    {
        public static List<T> From(byte[] data, int startPos, out int endPos)
        {
            int currentPosition = startPos;
            int length = SoxxaBitConverter.ToInt32(data, currentPosition);
            currentPosition = currentPosition + 4;
            var result = new List<T>();

            for (var i = 0; i < length; i++)
            {
                var entity = GetEntity(currentPosition, data, out currentPosition);
                currentPosition += entity.Size;
                result.Add(entity);
            }
            endPos = currentPosition;
            return result;
        }

        private static T GetEntity(int pos, byte[] data, out int endPos)
        {
            endPos = pos + 1;
            return (T)Activator.CreateInstance(typeof(T), new NullMask(data[pos]), data, pos + 1);
        }
    }
}
