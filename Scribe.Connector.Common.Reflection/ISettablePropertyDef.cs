namespace Scribe.Connector.Common.Reflection
{
    public interface ISettablePropertyDef
    {
        string Name { get; }

        string Description { get; }

        bool IsPrimaryKey { get; }

        bool UsedInQuerySelect { get; }

        bool UsedInQueryConstraint { get; }

        bool UsedInActionInput { get; }

        bool UsedInActionOutput { get; }

        bool UsedInLookupCondition { get; }

        bool UsedInQuerySequence { get; }

        bool RequiredInActionInput { get; }
    }
}