namespace Scribe.Connector.Common.Interfaces
{
    public interface IIndexedResult<out TValue> : IIndexed<TValue>, IResult
    {
    }
}