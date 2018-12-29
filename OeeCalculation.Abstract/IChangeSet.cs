using System.Collections.Generic;
namespace Production.Abstract
{
    public interface IDatabaseChangeSet
    {
        IDatabaseRecordSet Inserted { get; }
        IDatabaseRecordSet Updated { get; }
        IDatabaseRecordSet Deleted { get; }
        IEnumerable<ITrackable> Events { get; }
    }   
}
