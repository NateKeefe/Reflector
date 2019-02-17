namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    using Scribe.Core.ConnectorApi.Metadata;

    public abstract class ActionDef : IActionDefinition
    {
        private bool supportsRelations;

        /// <exception cref="System.NotSupportedException">
        /// Throws if the KnownAction is not a valid KnownAction, or if the
        /// KnownAction does not support the setting for
        /// <paramref name="supportsBulk" /> or does not support the setting for
        /// <paramref name="supportsMultiple"/> .
        /// </exception>
        protected ActionDef(
            string name,
            string description,
            KnownActions knownAction,
            bool supportsBulk,
            bool supportsMultiple)
        {
            if (knownAction == KnownActions.None || knownAction == KnownActions.InsertUpdate)
                throw new NotSupportedException($"{knownAction} is not a supported action type.");
            this.FullName = name;
            this.Name = name;
            this.Description = description;
            this.KnownActionType = knownAction;
            if (supportsBulk)
            {
                if (knownAction != KnownActions.Create || knownAction != KnownActions.CreateWith
                    || knownAction != KnownActions.Delete || knownAction != KnownActions.Update
                    || knownAction != KnownActions.UpdateWith
                    || knownAction != KnownActions.UpdateInsert)
                    throw new NotSupportedException($"{knownAction} does not support bulk.");
                this.SupportsBulk = true;
            }

            if (supportsMultiple)
            {
                if (knownAction != KnownActions.Delete || knownAction != KnownActions.Update
                    || knownAction != KnownActions.UpdateWith)
                    throw new NotSupportedException($"{knownAction} does not support multiple records.");
                this.SupportsMultipleRecordOperations = true;
            }

            this.SupportsInput = false;
            this.SupportsLookupConditions = false;
            this.supportsRelations = false;
            this.SupportsConstraints = false;
            this.SupportsSequences = false;
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public KnownActions KnownActionType { get; set; }

        public bool SupportsLookupConditions { get; set; }

        public bool SupportsInput { get; set; }

        public bool SupportsBulk { get; set; }

        public bool SupportsMultipleRecordOperations { get; set; }

        public bool SupportsSequences { get; set; }

        public bool SupportsConstraints { get; set; }

        public bool SupportsRelations
        {
            get => this.supportsRelations;
            set => this.supportsRelations = value;
        }
    }
}