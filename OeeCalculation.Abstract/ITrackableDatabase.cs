namespace Production.Abstract
{
    public delegate void Updated();
    public interface ITrackableDatabase
    {
        event Updated Updated;
    }
}
