using Production.Abstract;
namespace OeeCalculation.TrackableDatabaseContext
{
    public class TrackableDatabaseContext : ITrackableDatabaseContext
    {
        private readonly ITrackableDatabase trackableDatabase;
        private readonly IDatabaseContext databaseContext;
        public TrackableDatabaseContext(
            ITrackableDatabase trackableDatabase,
            IDatabaseContext databaseContext
            )
        {
            this.trackableDatabase = trackableDatabase;
            this.databaseContext = databaseContext;
        }
        public void Start()
        {
            databaseContext.Load();
           
        }
    }
}
