using System;
using System.Linq;
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

        public static Char ToChar(byte[] data, int offset)
        {
            //if (BitConverter.IsLittleEndian)
            //    return BitConverter.ToChar(BitConverter.IsLittleEndian ? data.Skip(offset).Take(1).Reverse().ToArray() : data, 0);
            return BitConverter.ToChar(data, offset);
        }

        public static Single ToSingle(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToSingle(BitConverter.IsLittleEndian ? data.Skip(offset).Take(8).Reverse().ToArray() : data, 0);
            return BitConverter.ToSingle(data, offset);
        }

        public static DateTime ToDateTime(byte[] data, int offset)
        {
            return DateTime.Parse("1900-01-01").AddDays(ToInt32(data, offset)).AddTicks(100 * ToInt32(data, offset + 4));
        }
    }
}
