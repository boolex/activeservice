namespace Production.Abstract
{
    /// <summary>
    /// Calculation service API
    /// </summary>
    public interface IProductionAttributeSource
    {
        object Get(ProductionAttribute[] attributes);
    }
}
