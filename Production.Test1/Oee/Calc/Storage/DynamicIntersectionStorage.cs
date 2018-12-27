using System.Collections.Generic;
using System.Linq;
namespace Production.Test1.Oee.Calc.Storage
{
    using Production.Abstract;
    using Production.Abstract;
    using Production.Test1.Oee.Calc;
    public class DynamicIntersectionStorage<T, F> : IDynamicStorage<Intersection<T, F>>
        where T : ICompletedProductionPeriod
        where F : ICompletedProductionPeriod
    {
        private readonly IDynamicStorage<T> a;
        private readonly IDynamicStorage<F> b;
        public DynamicIntersectionStorage(IDynamicStorage<T> a, IDynamicStorage<F> b)
        {
            this.a = a;
            this.b = b;
            this.history = new Intersections<T, F>(a.History, b.History).Get();
            StartUpdating();
        }
        protected IDynamicStorage<T> A { get { return a; } }
        protected IDynamicStorage<F> B { get { return b; } }

        private void StartUpdating()
        {
            a.Completed += a_Completed;
            b.Completed += b_Completed;
        }

        void b_Completed(F period)
        {
            if (this.a.Active != null && a.Active.StartTime < period.End)
            {
                history.Add(new Intersection<T, F>(a.Active, period));
            }
        }

        void a_Completed(T period)
        {
            if (b.Active != null && b.Active.StartTime < period.End)
            {
                history.Add(new Intersection<T, F>(period, b.Active));
            }
        }

        private List<Intersection<T, F>> history;
        public List<Intersection<T, F>> History
        {
            get { return history; }
        }

        private Intersections<T, F> active;
        public Intersection<T, F> Active
        {
            get
            {
                if (a.Active != null && b.Active != null)
                {
                    return new Intersection<T, F>(a.Active, b.Active);
                }
                return null;
            }
        }
        public void Start(Intersection<T, F> p)
        {
            throw new System.NotImplementedException();
        }

        public void End(Intersection<T, F> p)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Intersection<T, F>> All
        {
            get { return history.Union(activeList); }
        }

        private IEnumerable<Intersection<T, F>> activeList
        {
            get { return Active == null ? new List<Intersection<T, F>>() : new List<Intersection<T, F>> { Active }; }
        }

        public event CompletedPeriodDelegate<Intersection<T, F>> Completed;

        public event StartedPeriodDelegate<Intersection<T, F>> Started;
    }
}
