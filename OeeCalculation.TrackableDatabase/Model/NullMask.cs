namespace OeeCalculation.TrackableDatabase.Model
{
    public interface INullMask
    {
        bool this[int pos] { get; }
    }
    public class NullMask : INullMask
    {
        private readonly byte mask;
        public NullMask(byte mask)
        {
            this.mask = mask;
        }
        public bool this[int pos]
        {
            get
            {
                return (mask & (1 << pos)) != 0;
            }
        }
        public static INullMask Empty
        {
           get { return new NullMask(0); }
        }
    }
}
