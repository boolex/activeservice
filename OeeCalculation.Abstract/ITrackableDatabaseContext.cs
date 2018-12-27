namespace Production.Abstract
{
    public delegate void DatabaseChange();
    public class ITrackableDatabaseContext 
    {
        event DatabaseChange Updated;
    }
}
