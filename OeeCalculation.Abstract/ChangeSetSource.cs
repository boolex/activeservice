namespace Production.Abstract
{
    public delegate void Changed(IDatabaseChangeSet set);
    public interface IChangeSetSource
    {
        event Changed Changed;
    }
}
