using System.Collections.Generic;
using System.Linq;
using Production.Abstract;
namespace OeeCalculation.Computing.Production
{
    using OeeCalculation.Computing.ProductionEvents;
    public class SiteProduction : ISiteProduction
    {
        private readonly IChangeSetSource source;
        private readonly IComputingEventDataMapperFactory mapperFactory;
        public SiteProduction(
            IChangeSetSource source,
            IComputingEventDataMapperFactory mapperFactory)
        {
            this.source = source;
            this.mapperFactory = mapperFactory;
        }
        private void UpdateOperatorStationProduction(IMachine machine, IComputingEvent e)
        {
            OperatorStation(machine).Update(e);
        }
        private IProdplaceProduction Prodplace(IMachine machine)
        {
            return Prodplaces[machine.ProdPlaceId.Value];
        }
        public IOsProduction OperatorStation(IMachine machine)
        {
            int operorstationId;
            if (machine.OperatorStationId.HasValue)
            {
                operorstationId = machine.OperatorStationId.Value;
            }
            else
            {
                operorstationId = Pp2Os[machine.ProdPlaceId.Value];
            }

            return OperatorStations[operorstationId];
        }

        IProdplaceProduction ISiteProduction.Prodplace(IMachine machine)
        {
            throw new System.NotImplementedException();
        }

        private Dictionary<int, IOsProduction> operatorStations;
        public Dictionary<int, IOsProduction> OperatorStations
        {
            get { return operatorStations; }
        }
        private Dictionary<int, int> pp2Os;
        private Dictionary<int, int> Pp2Os
        {
            get
            {
                return pp2Os ?? (pp2Os = OperatorStations
                .SelectMany(x => x.Value.Prodplaces)
                .ToDictionary(x => x.Machine.ProdPlaceId.Value, y => y.Machine.OperatorStationId.Value));
            }
        }
        private Dictionary<int, IProdplaceProduction> prodplaces;
        public Dictionary<int, IProdplaceProduction> Prodplaces
        {
            get
            {
                return prodplaces ??
                    (prodplaces = OperatorStations
                    .SelectMany(x => x.Value.Prodplaces)
                    .ToDictionary(x => x.Machine.ProdPlaceId.Value, y => y));
            }
        }
        public void Listen()
        {
            source.Changed += (IDatabaseChangeSet set) =>
            {
                var siteChangeSet = new SiteChangeSet(this, set);
                siteChangeSet.OperatorStations.ToList().ForEach(x =>
                {
                    var mapper = mapperFactory.NewMapper(x.DbRecordsChanges, x.OperatorStation);
                    mapper.Events.ToList().ForEach(y =>
                    {
                        UpdateOperatorStationProduction(x.OperatorStation.Machine, y);
                    });
                });                
            };
        }
    }
}
