using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class TrackBinary :Track
    {
        public TrackBinary(byte[] data, int offset) :
           base(date: SoxxaBitConverter.ToDateTime(data, offset + 0),
                type: (TrackingType)SoxxaBitConverter.ToByte(data, offset + 8))
        {
        }
    }
}
