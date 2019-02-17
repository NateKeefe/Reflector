using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scribe.Connector.Common.Interfaces;

namespace Scribe.Connector.Common.Operation
{
    public static class ResultItemExt
    {
        public static IEnumerable<IIndexedResult<TIn>> Convert<TIn>(
            this IEnumerable<InputItem> source, Func<InputItem, TIn> func)
        {
            return ResultItemOperators.Convert(source, func);
        }

        public static IEnumerable<IIndexedResult<U>> ExecuteBundle<T, U>(
            this IEnumerable<IIndexedResult<T>> source, int bundleSize, Func<IList<T>, Task<IList<U>>> func)
        {
            var list = new List<IIndexedResult<T>>(bundleSize);

            foreach (var item in source)
            {
                if (item.HasError)
                {
                    yield return new IndexedResult<U>(new Indexed<U>(default(U), item.Index), item);
                }

                list.Add(item);

                if (list.Count == bundleSize)
                {
                    var results = func(list.Select(indx => indx.Value).ToList());
                    list = new List<IIndexedResult<T>>(bundleSize);
                    var zipped = results.Result.ToList()
                        .Zip(list, (a, b) => new IndexedResult<U>(new Indexed<U>(a, b.Index)));

                    foreach (var indexedResult in zipped)
                    {
                        yield return indexedResult;
                    }
                }
            }
        }

        public static IEnumerable<IList<string>> Test(IEnumerable<string> source, int num)
        {
            var e = source.GetEnumerator();
            var bundle = new List<string>(num);
            bool has;
            do
            {
                bool bundlefyIt = false;
                has = e.MoveNext();
                if (has)
                {
                    bundle.Add(e.Current);

                    if (bundle.Count == num)
                    {
                        bundlefyIt = true;
                    }
                }
                else
                {
                    if (bundle.Count > 0)
                    {
                        bundlefyIt = true;
                    }
                }

                if (bundlefyIt)
                {
                    yield return bundle;
                    if (has) bundle = new List<string>(num);
                }
            }
            while (has);
        }
    }
}