namespace Scribe.Connector.Common.Operation
{
    public interface IHasValue<out T>
    {
        T Value { get; }
    }
}