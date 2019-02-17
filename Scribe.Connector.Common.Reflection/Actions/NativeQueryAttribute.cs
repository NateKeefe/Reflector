namespace Scribe.Connector.Common.Reflection.Actions
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class NativeQueryAttribute : Attribute
    {
        public ActionDef ToActionDefinition()
        {
            return new NativeQueryAction();
        }
    }
}