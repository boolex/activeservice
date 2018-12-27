using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace Production
{
    public class IntersectionSet : IDataReader
    {
        private int index = -1;
        private readonly IEnumerable<Intersection> intersections;
        public IntersectionSet(IEnumerable<Intersection> intersections)
        {
            this.intersections = intersections;
        }
        public bool Read()
        {
            if (intersections.Count() - 1 == index)
            {
                return false;
            }
            else
            {
                index++;
                return true;
            }
        }
        public int FieldCount
        {
            get { return 6; }
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsClosed {get { throw new NotImplementedException();}
        }

        public int RecordsAffected {get {throw new NotImplementedException();}
        }

        public object this[string name] {get { throw new NotImplementedException();}
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        public object GetValue(int i)
        {
            return new object[]
            {
                intersections.ElementAt(index).ProdplaceId,
                intersections.ElementAt(index).CalendarId,
                intersections.ElementAt(index).OrderId,
                intersections.ElementAt(index).DtoId,
                intersections.ElementAt(index).From,
                intersections.ElementAt(index).To
            }[i];
        }

        public void Close()
        {

        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }
    }
}
