namespace Scribe.Connector.Common.Reflection
{
    using System;

    public interface IReflectionSpecification
    {
        
        Func<Type, bool> TypeFilter { get; }

        Func<Type, ObjDefStub> GetObjectDefFromType { get; }
    }
}