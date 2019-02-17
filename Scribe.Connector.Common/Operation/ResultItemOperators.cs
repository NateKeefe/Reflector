using System;
using System.Collections.Generic;
using System.Linq;
using Scribe.Connector.Common.Interfaces;

namespace Scribe.Connector.Common.Operation
{
    public static class ResultItemOperators
    {
        public static IEnumerable<IIndexedResult<TIn>> Convert<TIn>(IEnumerable<InputItem> source, Func<InputItem, TIn> func)
        {
            return source.Select((item, i) => TryConvert(item, i, func));
        }

        private static IIndexedResult<TIn> TryConvert<TIn>(
            InputItem item, int i, Func<InputItem, TIn> converter)
        {
            try
            {
                var converted = converter(item);
                return new IndexedResult<TIn>(new Indexed<TIn>(converted, i));
            }
            catch (Exception ex)
            {
                return new IndexedResult<TIn>(new IndexedResult<TIn>(new Indexed<TIn>(default(TIn), i), new Result(ex)));
            }
        }
    }
}