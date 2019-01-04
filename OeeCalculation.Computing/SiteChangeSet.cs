using System.Collections.Generic;
using System.Linq;
using Production.Abstract;
namespace OeeCalculation.Computing
{
    using OeeCalculation.Computing.Production;
    public class SiteChangeSet
    {
        private readonly ISiteProduction site;
        private readonly IDatabaseChangeSet changeSet;
        public SiteChangeSet(
            ISiteProduction site,
            IDatabaseChangeSet changeSet)
        {
            this.site = site;
            this.changeSet = changeSet;
        }
        private IEnumerable<OsChangeSet> operatorStations;
        public IEnumerable<OsChangeSet> OperatorStations
        {
            get
            {
                return operatorStations ??
                    (operatorStations = changeSet
                    .Events
                    .GroupBy(x => site.OperatorStation(x.Machine))
                    .Select(y => new OsChangeSet(y.Key, y.ToList())));
            }
        }
    }
    public class OsChangeSet
    {
        private readonly IOsProduction operatorstation;
        private readonly IEnumerable<ITrackable> dbRecordsChanges;
        public OsChangeSet(IOsProduction operatorstation, IEnumerable<ITrackable> dbRecordsChanges)
        {
            this.operatorstation = operatorstation;
            this.dbRecordsChanges = dbRecordsChanges;
        }
        public IOsProduction OperatorStation { get { return operatorstation; } }
        public IEnumerable<ITrackable> DbRecordsChanges { get { return dbRecordsChanges; } }
    }
}
