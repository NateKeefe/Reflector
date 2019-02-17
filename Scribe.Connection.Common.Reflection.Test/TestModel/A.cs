using System.Collections.Generic;
using System.Xml.Serialization;
using Scribe.Connector.Common.Reflection;



public class Rootobject
{
    public Class1[] Property1 { get; set; }
}

[ObjectDefinition]
public class Class1
{
    public string entityName { get; set; }
    public string verb { get; set; }
    public string jsonSample { get; set; }
}


namespace Scribe.Connection.Common.Reflection.Test.TestModel
{
    [ObjectDefinition(Name = "WasA")]
    public class A
    {
        [PropertyDefinition]
        public A SingleA { get; set; }

        [PropertyDefinition]
        public B SingleB { get; set; }

        [PropertyDefinition]
        public string Str { get; set; }

        [PropertyDefinition]
        public List<A> ManyAs { get; set; }

        [PropertyDefinition]
        public IEnumerable<string> ManyStrs { get; set; }
    }

    [ObjectDefinition(Name = "WasB")]
    public class B
    {
        public A SingleA { get; set; }
    }

    [ObjectDefinition(Name = "WasC")]
    public class C
    {
        //[PropertyDefinition]
        //public IList<string> IListStrings { get; set; }

        [PropertyDefinition]
        public List<string> ListStrings { get; set; }

        [PropertyDefinition]
        public IEnumerable<string> IEStrings { get; set; }

        [PropertyDefinition]
        public string[] StringArray { get; set; }
    }
}