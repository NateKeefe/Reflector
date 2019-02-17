using System;

namespace Scribe.Connector.Common.Interfaces
{
    public interface IResult
    {
        Exception Error { get; }
        bool HasError { get; }
        bool IsFatalError { get; }
        int ObjectsEffected { get; }
    }
}