using Production.Abstract;
using Production.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Production.Test1.Oee.Calc.Storage
{
    public class DynamicStorage<T> : IDynamicStorage<T> where T : class, ICompletedProductionPeriod
    {
        private T active;
        private readonly List<T> history;
        public DynamicStorage(List<T> history = null)
        {
            if (history != null)
            {
                this.history = history;
            }
            else
            {
                this.history = new List<T>();
            }
        }
        public List<T> History
        {
            get
            {
                return history;
            }
        }
        public T Active { get { return active; } private set { active = value; } }
        public void Start(T p)
        {
            Active = p;
            if (Started != null)
            {
                Started(p);
            }
        }
        public void End(T p)
        {
            Active = null;
            history.Add(p);
            if (Completed != null)
            {
                Completed(p);
            }
        }

        public IEnumerable<T> All
        {
            get { return history.Union(activeList); }
        }

        private IEnumerable<T> activeList
        {
            get { return Active == null ? new List<T>() : new List<T> { Active }; }
        }

        public event CompletedPeriodDelegate<T> Completed;

        public event StartedPeriodDelegate<T> Started;
    }
}
