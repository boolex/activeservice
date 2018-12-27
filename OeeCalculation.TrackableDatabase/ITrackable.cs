using OeeCalculation.TrackableDatabase.Model;
namespace OeeCalculation.TrackableDatabase
{
    public interface ITrackable
    {
        INullMask Nulls { get; }
        Track Track { get; }  
        int Size { get; }
    }
}
