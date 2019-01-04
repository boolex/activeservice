using Production.Abstract;
using System.Collections.Generic;
using System.Linq;
namespace OeeCalculation.Computing.Storage
{
    public class Intersections<T, F>
        where T : ICompletedProductionPeriod
        where F : ICompletedProductionPeriod
    {
        private readonly IOrderedEnumerable<T> a;
        private readonly IOrderedEnumerable<F> b;
        public Intersections(IEnumerable<T> a, IEnumerable<F> b)
        {
            this.a = a.OrderBy(x => x.StartTime);
            this.b = b.OrderBy(x => x.StartTime);
        }
        private Dictionary<T, List<F>> result;
        public Dictionary<T, List<F>> Result
        {
            get
            {
                return result ?? (result = findIntersections());
            }
        }
        public List<Intersection<T, F>> Get()
        {
            return Result.SelectMany(x => Result[x.Key].Select(y => new Intersection<T, F>(x.Key, y))).ToList();
        }
        private Dictionary<T, List<F>> findIntersections()
        {
            var aEnum = a.GetEnumerator();
            var bEnum = b.GetEnumerator();
            bool nonEmptyList = true;
            nonEmptyList = aEnum.MoveNext();
            nonEmptyList = bEnum.MoveNext();
            Dictionary<T, List<F>> intersections = new Dictionary<T, List<F>>();

            while (nonEmptyList && bEnum.Current != null && aEnum.Current != null)
            {
                if (bEnum.Current.End.HasValue && aEnum.Current.StartTime > bEnum.Current.End)
                {
                    nonEmptyList = bEnum.MoveNext();
                }
                else if (aEnum.Current.End.HasValue && bEnum.Current.StartTime > aEnum.Current.End)
                {
                    nonEmptyList = aEnum.MoveNext();
                }
                else
                {
                    if (!intersections.ContainsKey(aEnum.Current))
                    {
                        intersections.Add(aEnum.Current, new List<F>());
                    }
                    intersections[aEnum.Current].Add(bEnum.Current);

                    if (bEnum.Current.End > aEnum.Current.End)
                    {
                        nonEmptyList = aEnum.MoveNext();
                    }
                    else
                    {
                        nonEmptyList = bEnum.MoveNext();
                    }
                }
            }
            return intersections;
        }
    }
}
