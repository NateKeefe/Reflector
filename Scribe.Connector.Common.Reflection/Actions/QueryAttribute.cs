namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class QueryAttribute : Attribute
    {
        public ActionDef ToActionDefinition()
        {
            return new QueryAction();
        }
    }
}