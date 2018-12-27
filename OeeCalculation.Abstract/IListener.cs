namespace Production.Abstract
{
    public interface IListener<T>
    {
        event System.Action<T> Change;
    }
}
