namespace Scribe.Connector.Common.Reflection.Actions
{
    public class UpsertAttribute : SupportedOperationAttribute
    {
        public bool SupportsMulti { get; set; } = false;

        public bool SupportsBulk { get; set; } = false;

        public string Description { get; set; } = "Upsert Action.";

        public string Name { get; set; } = "Upsert";

        public override ActionDef ToActionDefinition()
        {
            return new UpsertAction(this.Name, this.Description, this.SupportsBulk);
        }
    }
}