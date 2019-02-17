namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = true)]
    public class UpdateWithAttribute : SupportedOperationAttribute
    {
        public bool SupportsMulti { get; set; } = false;

        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "UpdateWith Action.";

        public string Name { get; set; } = "UpdateWith";

        public override ActionDef ToActionDefinition()
        {
            return new UpdateWithAction(this.Name, this.Description, this.SupportsBulk, this.SupportsMulti);
        }
    }
}