using System;
namespace Production.Abstract.Model
{
    public class DowntimeOccasion : IProdPlaceEntity, ICompletedProductionPeriod
    {
        private int prodPlaceId;
        private int id;
        private DateTime start;
        private DateTime? end;
        private int? lossType;
        public DowntimeOccasion() { }
        public DowntimeOccasion(
            int prodPlaceId,
            int id,
            DateTime start,
            DateTime? end = null,
            int? lossType = null)
        {
            this.prodPlaceId = prodPlaceId;
            this.id = id;
            this.start = start;
            this.end = end;
            this.lossType = lossType;
        }
        public int ProdPlace_Id { get { return prodPlaceId; } protected set { prodPlaceId = value; } }
        public int DTOccasion_Id { get { return id; } protected set { id = value; } }
        public DateTime BeginTime { get { return start; } protected set { start = value; } }
        public DateTime? EndTime { get { return end; } protected set { end = value; } }
        public int? LossType { get { return lossType; } protected set { lossType = value; } }
        public DateTime? End
        {
            get
            {
                return EndTime;
            }
        }
        public DateTime StartTime
        {
            get { return BeginTime; }
        }
    }
}
