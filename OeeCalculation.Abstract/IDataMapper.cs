using System.Collections.Generic;
namespace Production.Abstract
{
    public interface IDataMapper
    {
        IEnumerable<IProductionEvent> Events { get; }
    }
}
