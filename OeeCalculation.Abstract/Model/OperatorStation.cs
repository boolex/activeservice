using System.Collections.Generic;
namespace Production.Abstract.Model
{
    public class OperatorStation
    {
        private readonly int id;
        private readonly List<CalendarHistory> calendar;
        private readonly List<ProdPlace> prodplaces;
        private readonly List<Order> orders;
        public OperatorStation(
            int id,
            List<CalendarHistory> calendar,
            List<ProdPlace> prodplaces,
            List<Order> orders)
        {
            this.calendar = calendar;
            this.id = id;
            this.prodplaces = prodplaces;
            this.orders = orders;
        }
        public int OperatorStation_Id { get { return id; } }
        public List<CalendarHistory> Calendar { get { return calendar; } }
        public List<ProdPlace> ProdPlaces { get { return prodplaces; } }
        public List<Order> Orders { get { return orders; } }
    }
}
