namespace Scribe.Connector.Common.Reflection.Actions
{
    public class DeleteAttribute : SupportedOperationAttribute
    {
        public bool SupportsMulti { get; set; } = false;

        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "Delete Action.";

        public string Name { get; set; } = "Delete";

        public override ActionDef ToActionDefinition()
        {
            return new DeleteAction(this.Name, this.Description, this.SupportsBulk, this.SupportsMulti);
        }
    }
}