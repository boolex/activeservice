using System.Collections.Generic;
namespace Production.Abstract.Model
{
    public class ProdPlace : IOperatorStationEntity
    {
        private readonly int operatorStationId;
        private readonly int id;
        private readonly List<DowntimeOccasion> stops;
        public ProdPlace(
            int operatorStationId,
            int id,
            List<DowntimeOccasion> stops)
        {
            this.operatorStationId = operatorStationId;
            this.id = id;
            this.stops = stops;
        }
        public int OperatorStation_Id { get { return operatorStationId; } }
        public int Id { get { return id; } }
        public List<DowntimeOccasion> Stops { get { return stops; } }
    }
}
