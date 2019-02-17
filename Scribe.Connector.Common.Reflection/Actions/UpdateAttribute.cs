namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = true)]
    public class UpdateAttribute : SupportedOperationAttribute
    {
        public bool SupportsMulti { get; set; } = false;

        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "Update Action.";

        public string Name { get; set; } = "Update";

        public override ActionDef ToActionDefinition()
        {
            return new UpdateAction(this.Name, this.Description, this.SupportsBulk, this.SupportsMulti);
        }
    }
}