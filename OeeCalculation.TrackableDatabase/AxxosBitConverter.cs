using System;
using System.Linq;
using OeeCalculation.TrackableDatabase.Model;

namespace OeeCalculation.TrackableDatabase
{
    public class AxxosBitConverter
    {
        public static Int16 ToInt16(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt16(BitConverter.IsLittleEndian ? data.Skip(offset).Take(2).Reverse().ToArray() : data, 0);
            return BitConverter.ToInt16(data, offset);
        }
        public static Int32 ToInt32(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt32(BitConverter.IsLittleEndian ? data.Skip(offset).Take(4).Reverse().ToArray() : data, 0);
            return BitConverter.ToInt32(data, offset);
        }
        public static Int64 ToInt64(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt64(BitConverter.IsLittleEndian ? data.Skip(offset).Take(8).Reverse().ToArray() : data, 0);
            return BitConverter.ToInt64(data, offset);
        }

        public static Byte ToByte(byte[] data, int offset)
        {
            //if (BitConverter.IsLittleEndian)
            //    return BitConverter.ToChar(BitConverter.IsLittleEndian ? data.Skip(offset).Take(1).Reverse().ToArray() : data, 0);
            return data[offset];
        }

        public static Single ToSingle(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToSingle(BitConverter.IsLittleEndian ? data.Skip(offset).Take(8).Reverse().ToArray() : data, 0);
            return BitConverter.ToSingle(data, offset);
        }

        public static DateTime ToDateTime(byte[] data, int offset)
        {
            return DateTime.Parse("2000-01-01").AddDays(ToInt32(data, offset)).AddMilliseconds(ToInt32(data, offset + 4));
        }
        public static DateTime? ToDateTimeNullable(byte[] data, int offset, INullMask mask, int nullablePosition)
        {
            if (mask[nullablePosition])
            {
                return null;
            }
            return ToDateTime(data, offset);
        }
        public static Int32? ToInt32Nullable(byte[] data, int offset, INullMask mask, int nullablePosition)
        {
            if (mask[nullablePosition])
            {
                return null;
            }
            return ToInt32(data, offset);
        }
    }
}
