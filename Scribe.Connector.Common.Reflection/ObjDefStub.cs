namespace Scribe.Connector.Common.Reflection
{
    public class ObjDefStub
    {
        public ObjDefStub(string name, string description, bool hidden)
        {
            this.Name = name;
            this.Description = description;
            this.Hidden = hidden;
        }

        public string Name { get; }

        public string Description { get; }

        public bool Hidden { get; }
    }
}