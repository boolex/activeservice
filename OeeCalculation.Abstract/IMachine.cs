namespace Production.Abstract
{
    public interface IMachine
    {
        int? ProdPlaceId { get; }
        int? OperatorStationId { get; }
        int? OrderId { get; }
    }
    public class Machine : IMachine
    {
        private readonly int? operatorStationId;
        private readonly int? prodplaceId;
        private readonly int? orderId;
        public Machine(int? operatorStationId = null, int? prodplaceId = null, int? orderId = null)
        {
            this.operatorStationId = operatorStationId;
            this.prodplaceId = prodplaceId;
            this.orderId = orderId;
        }
        public int? ProdPlaceId
        {
            get { return prodplaceId; }
        }
        public int? OperatorStationId
        {
            get { return operatorStationId; }
        }
        public int? OrderId
        {
            get { return orderId; }
        }
    }
}
