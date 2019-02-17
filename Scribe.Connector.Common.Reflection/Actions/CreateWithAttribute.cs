namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = true)]
    public class CreateWithAttribute : SupportedOperationAttribute
    {
        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "CreateWith Action.";

        public string Name { get; set; } = "CreateWith";

        public override ActionDef ToActionDefinition()
        {
            return new CreateWithAction(this.Name, this.Description, this.SupportsBulk);
        }
    }
}