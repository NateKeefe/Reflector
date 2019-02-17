namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = true)]
    public class CreateAttribute : SupportedOperationAttribute
    {
        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "Create Action.";

        public string Name { get; set; } = "Create";

        public override ActionDef ToActionDefinition()
        {
            return new CreateAction(this.Name, this.Description, this.SupportsBulk);
        }
    }
}