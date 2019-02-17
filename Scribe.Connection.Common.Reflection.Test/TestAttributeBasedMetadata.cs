namespace Scribe.Connection.Common.Reflection.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Scribe.Connector.Common.Reflection;
    using Scribe.Connector.Common.Reflection.Actions;
    using Scribe.Core.ConnectorApi;

    [TestClass]
    public class TestAttributeBasedMetadata
    {

        public static IMetadata m = MetadataReflector.ReflectMetadata(typeof(Y));
        public static IMetadataProvider mp = MetadataReflector.ReflectMetadata(typeof(Y)).Wrap();

        [TestMethod]
        public void TestFullMetadata()
        {

            var objs = mp.RetrieveObjectDefinitions().ToList();

            var actions = mp.RetrieveActionDefinitions().ToList();

            Assert.AreEqual(17, objs.Count);
            Assert.AreEqual(2, actions.Count);
        }

        [TestMethod]
        public void TestPropertyCollections()
        {
            var od = MetadataReflector.Define<Y>();

            Assert.AreEqual("Y", od.FullName);
            Assert.AreEqual(3, od.PropertyDefinitions.Count);

            var stringA = od.PropertyDefinitions.Single(pd => pd.FullName == "StringA");
            Assert.AreEqual("StringA", stringA.FullName, "The name should be StringA.");
            Assert.AreEqual("System.String", stringA.PropertyType);
            Assert.AreEqual(1, stringA.MaxOccurs);

            var strings = od.PropertyDefinitions.Single(pd => pd.FullName == "MyStrings");

            Assert.AreEqual("MyStrings", strings.FullName, "The name should be MyStrings.");
            Assert.AreEqual("System.String", strings.PropertyType);
            Assert.AreEqual(-1, strings.MaxOccurs);

            var refProp = od.PropertyDefinitions.Single(pd => pd.FullName == "XXX");
            Assert.AreEqual("MyX", refProp.PropertyType);
            Assert.AreEqual(1, refProp.MaxOccurs);
        }

        [TestMethod]
        public void TestReflectMany()
        {
            var ods = MetadataReflector.ReflectMetadata(typeof(X)).Types.Values.ToList();

            Assert.AreEqual(17, ods.Count);

            // Make sure all of the other tests pass as well
            var od = ods.Single(o => o.FullName == "Y");

            Assert.AreEqual("Y", od.FullName);
            Assert.AreEqual(3, od.PropertyDefinitions.Count);

            var stringA = od.PropertyDefinitions.Single(pd => pd.FullName == "StringA");
            Assert.AreEqual("StringA", stringA.FullName);
            Assert.AreEqual("System.String", stringA.PropertyType);
            Assert.AreEqual(1, stringA.MaxOccurs);

            var strings = od.PropertyDefinitions.Single(pd => pd.FullName == "MyStrings");

            Assert.AreEqual("MyStrings", strings.FullName);
            Assert.AreEqual("System.String", strings.PropertyType);
            Assert.AreEqual(-1, strings.MaxOccurs);

            var refProp = od.PropertyDefinitions.Single(pd => pd.FullName == "XXX");
            Assert.AreEqual("MyX", refProp.PropertyType);
            Assert.AreEqual(1, refProp.MaxOccurs);
        }

        [TestMethod]
        public void TestSupportsQuery()
        {
            var od = MetadataReflector.Define<X>();

            Assert.AreEqual(1, od.SupportedActionFullNames.Count);
        }

        [TestMethod]
        public void TestRequiresWorks()
        {
            var od = MetadataReflector.Define<Xyz>();
            var prop = od.PropertyDefinitions.First();
            Assert.AreEqual(true, prop.RequiredInActionInput);
        }

        [TestMethod]
        public void TestSupportsUpdate()
        {
            var od = MetadataReflector.Define<Y>();

            Assert.AreEqual(2, od.SupportedActionFullNames.Count);
        }

        [TestMethod]
        public void TestUseAttributeNameForObjectDefName()
        {
            var od = MetadataReflector.Define<X>();

            Assert.AreEqual("MyX", od.FullName);
            Assert.AreEqual(1, od.PropertyDefinitions.Count);
            Assert.AreEqual("StringA", od.PropertyDefinitions.First().FullName);
            Assert.AreEqual("System.String", od.PropertyDefinitions.First().PropertyType);
        }

        [TestMethod]
        public void TestDefaultPropertyAttributes()
        {
            var od = MetadataReflector.Define<TestAllAttributes>();

            var p = od.PropertyDefinitions.Single(pd => pd.FullName == "a");

            Assert.AreEqual(false, p.RequiredInActionInput, "Default for Required is false.");
            Assert.AreEqual(true, p.UsedInActionInput, "Default for Input is true.");
            Assert.AreEqual(true, p.UsedInActionOutput, "Default for Output is true.");
            Assert.AreEqual(false, p.UsedInLookupCondition, "Default for TargetFilter(LookupCondition) is false.");
            Assert.AreEqual(false, p.UsedInQueryConstraint, "Default for QueryFilter(QueryConstraint) is false.");
            Assert.AreEqual(true, p.UsedInQuerySelect, "Default for Selectable is true.");

            // Questionable -- probably should be false
            Assert.AreEqual(true, p.UsedInQuerySequence, "Default for Orderable by (QuerySequence) is true.");
            
            Assert.AreEqual(string.Empty, p.Description, "Default for Description is an empty string.");
            Assert.AreEqual(p.FullName, p.Name, "Name always should equal FullName.");
            Assert.AreEqual(p.FullName, p.Name, "Name always should equal FullName.");
            Assert.AreEqual(0, p.Size, "Size is 0.");
        }

        [TestMethod]
        public void TestRequiredPropertyAttributes()
        {
            var od = MetadataReflector.Define<TestAllAttributes>();

            var p = od.PropertyDefinitions.Single(pd => pd.FullName == "b");

            Assert.AreEqual(true, p.RequiredInActionInput, "Overrided Required should take precedence.");
            Assert.AreEqual(true, p.UsedInActionInput, "Default for Input is true.");
            Assert.AreEqual(true, p.UsedInActionOutput, "Default for Output is true.");
            Assert.AreEqual(false, p.UsedInLookupCondition, "Default for TargetFilter(LookupCondition) is false.");
            Assert.AreEqual(false, p.UsedInQueryConstraint, "Default for QueryFilter(QueryConstraint) is false.");
            Assert.AreEqual(true, p.UsedInQuerySelect, "Default for Selectable is true.");

            // Questionable -- probably should be false
            Assert.AreEqual(true, p.UsedInQuerySequence, "Default for Orderable by (QuerySequence) is true.");

            // Questionable -- probably should be empty string
            Assert.AreEqual("", p.Description, "Default for Description is an empty string.");
            Assert.AreEqual(p.FullName, p.Name, "Name always should equal FullName.");
            Assert.AreEqual(p.FullName, p.Name, "Name always should equal FullName.");
            Assert.AreEqual(0, p.Size, "Size is 0.");
        }
    }

    [ObjectDefinition(Name = "MyX")]
    [Query]
    public class X
    {
        [PropertyDefinition(UsedInQuerySelect = true)]
        public string StringA { get; set; }
    }

    [ObjectDefinition]
    [Query]
    [Update]
    public class Y
    {
        [PropertyDefinition]
        public List<string> MyStrings { get; set; }

        [PropertyDefinition(Name = "XXX")]
        public X MyX { get; set; }

        [PropertyDefinition]
        public string StringA { get; set; }
    }

    [ObjectDefinition]
    public class Xyz
    {
        [PropertyDefinition(RequiredInActionInput = true)]
        public int Id { get; set; }
    }

    [ObjectDefinition]
    public class TestAllAttributes
    {
        [PropertyDefinition()]
        public int a { get; set; }

        [PropertyDefinition(RequiredInActionInput = true)]
        public int b { get; set; }

        [PropertyDefinition]
        public int c { get; set; }

        [PropertyDefinition]
        public int d { get; set; }
    }
}