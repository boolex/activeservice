using OeeCalculation.TrackableDatabase.Model;
namespace OeeCalculation.TrackableDatabase
{
  
    public interface IDeserializableDbRecord
    {
        int Size { get; }
        INullMask Nulls { get; }
    }
}
