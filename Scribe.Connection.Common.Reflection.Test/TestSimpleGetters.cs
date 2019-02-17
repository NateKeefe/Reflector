namespace Scribe.Connection.Common.Reflection.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Scribe.Connection.Common.Reflection.Test.TestModel;
    using Scribe.Connector.Common.Reflection;
    using Scribe.Connector.Common.Reflection.Data;
    using Scribe.Core.ConnectorApi.Query;

    [TestClass]
    public class TestToDataEntity
    {
        [TestMethod]
        public void TestSimpleProps()
        {
            var a = new TestModel.A();

            a.Str = "asdf";

            var de = DataReflector.ToDataEntity(
                a,
                new QueryEntity() { ObjectDefinitionFullName = "WasA", PropertyList = new List<string> { "Str" } });

            Assert.AreEqual(1, de.Properties.Count);
            Assert.AreEqual("asdf", de.Properties["Str"]);
            Assert.AreNotEqual("_asdf", de.Properties["Str"]);
        }

        [TestMethod]
        public void TestSimplePropsWithSimpleList()
        {
            var a = new TestModel.A();

            a.Str = "asdf";
            a.ManyStrs = new List<string> { "asdf", "Zxcv" };

            var de = DataReflector.ToDataEntity(
                a,
                new QueryEntity()
                    {
                        ObjectDefinitionFullName = "WasA",
                        PropertyList = new List<string> { "Str", "ManyStrs" }
                    });

            Assert.AreEqual(2, de.Properties.Count);
            Assert.AreEqual("asdf", ((List<string>)de.Properties["ManyStrs"]).First());
        }

        [TestMethod]
        public void TestComplexProp()
        {
            var a = new TestModel.A();

            var innerA = new TestModel.A { Str = "qqq", ManyStrs = new List<string> { "qwer", "cvbn" } };

            a.Str = "xxx";
            a.ManyStrs = new List<string> { "asdf", "Zxcv" };
            a.SingleA = innerA;

            var qe = new QueryEntity
                         {
                             ObjectDefinitionFullName = "WasA",
                             PropertyList = new List<string> { "Str", "ManyStrs" },
                             ChildList = new List<QueryEntity>
                                             {
                                                 new QueryEntity
                                                     {
                                                         ObjectDefinitionFullName
                                                             = "WasA",
                                                         PropertyList =
                                                             new List<string
                                                                 >
                                                                     {
                                                                         "Str",
                                                                         "ManyStrs"
                                                                     },
                                                         Name = "SingleA"
                                                     }
                                             }
                         };

            var de = DataReflector.ToDataEntity(a, qe);

            Assert.AreEqual(2, de.Properties.Count);
            Assert.AreEqual("asdf", ((List<string>)de.Properties["ManyStrs"]).First());
            Assert.AreEqual(1, de.Children.Count);
            Assert.AreEqual("WasA", de.Children["SingleA"].First().ObjectDefinitionFullName);

            Assert.AreEqual("qqq", de.Children["SingleA"].First().Properties["Str"]);
        }

        [TestMethod]
        public void TestComplexPropsThatAreNull()
        {
            var a = new TestModel.A();

            var innerA = new TestModel.A { Str = "qqq", ManyStrs = new List<string> { "qwer", "cvbn" } };

            a.Str = "xxx";
            a.ManyStrs = new List<string> { "asdf", "Zxcv" };
            a.SingleA = null;

            var qe = new QueryEntity
                         {
                             ObjectDefinitionFullName = "WasA",
                             PropertyList = new List<string> { "Str", "ManyStrs" },
                             ChildList = new List<QueryEntity>
                                             {
                                                 new QueryEntity
                                                     {
                                                         ObjectDefinitionFullName
                                                             = "WasA",
                                                         PropertyList =
                                                             new List<string
                                                                 >
                                                                     {
                                                                         "Str",
                                                                         "ManyStrs"
                                                                     },
                                                         Name = "SingleA"
                                                     }
                                             }
                         };

            var de = DataReflector.ToDataEntity(a, qe);

            Assert.AreEqual(2, de.Properties.Count);
            Assert.AreEqual("asdf", ((List<string>)de.Properties["ManyStrs"]).First());
            Assert.AreEqual(1, de.Children.Count);
            Assert.AreEqual(1, de.Children.First().Value.Count, "A Single Complex Property should always be encoded as a list of 1.");
            Assert.IsNull(de.Children["SingleA"].First());
        }

        [TestMethod]
        public void TestComplexPropLists()
        {
            var a = new TestModel.A();

            var innerA = new TestModel.A { Str = "qqq", ManyStrs = new List<string> { "qwer", "cvbn" } };

            a.Str = "xxx";
            a.ManyStrs = new List<string> { "asdf", "Zxcv" };
            a.SingleA = innerA;
            a.ManyAs = new List<TestModel.A> { innerA, innerA };

            var qe = new QueryEntity
                         {
                             ObjectDefinitionFullName = "WasA",
                             PropertyList = new List<string> { "Str", "ManyStrs" },
                             ChildList = new List<QueryEntity>
                                             {
                                                 new QueryEntity
                                                     {
                                                         ObjectDefinitionFullName
                                                             = "WasA",
                                                         PropertyList =
                                                             new List<string
                                                                 >
                                                                     {
                                                                         "Str",
                                                                         "ManyStrs"
                                                                     },
                                                         Name = "SingleA"
                                                     },
                                                 new QueryEntity
                                                     {
                                                         ObjectDefinitionFullName
                                                             = "WasA",
                                                         PropertyList =
                                                             new List<string
                                                                 >
                                                                     {
                                                                         "Str",
                                                                         "ManyStrs"
                                                                     },
                                                         Name = "ManyAs"
                                                     }
                                             }
                         };

            var de = DataReflector.ToDataEntity(a, qe);

            Assert.AreEqual(2, de.Properties.Count);
            Assert.AreEqual("asdf", ((List<string>)de.Properties["ManyStrs"]).First());
            Assert.AreEqual(2, de.Children.Count);
            Assert.AreEqual("WasA", de.Children["SingleA"].First().ObjectDefinitionFullName);

            Assert.AreEqual("qqq", de.Children["SingleA"].First().Properties["Str"]);
            Assert.AreEqual(2, de.Children["ManyAs"].Count);

            var childrenNested = de.Children["ManyAs"];
            Assert.IsTrue(
                childrenNested.SelectMany(x => x.Properties.Where(kv => kv.Key == "Str"))
                    .All(kv => kv.Value.ToString() == "qqq"));
        }
    }



    [ObjectDefinition]
    public class Q
    {
        [PropertyDefinition]
        public string Str { get; set; }

        [PropertyDefinition]
        public int Int { get; set; }

        [PropertyDefinition]
        public int? NInt { get; set; }
    }

[TestClass]
    public class TestSimpleGetters
    {
        [PropertyDefinition(Name = "NI")]
        public int? NullableInt { get; set; }

        [PropertyDefinition]
        public string Str { get; set; }

        [PropertyDefinition]
        public string[] StringArray { get; set; }

        [PropertyDefinition]
        public IEnumerable<string> StringE { get; set; }

        [PropertyDefinition]
        public IEnumerable<int?> MaybeInts { get; set; }

        [PropertyDefinition]
        public int?[] MaybeIntArray { get; set; }

        [TestMethod]
        public void TestMaybeIntArrayTurnedToLists()
        {
            this.MaybeIntArray = new int?[] { null, 3, 4 };

            var getter = DataReflector.BuildGetter(this.GetType().GetProperty("MaybeIntArray"));

            var name = getter.Key;

            var getterF = getter.Value;

            Assert.AreEqual("MaybeIntArray", name);
            var val = getterF(this);
            Assert.IsInstanceOfType(val, typeof(List<int?>));
        }

        [TestMethod]
        public void TestMaybeIntsTurnedToLists()
        {
            this.MaybeInts = new int?[] { null,3,4 };

            var getter = DataReflector.BuildGetter(this.GetType().GetProperty("MaybeInts"));

            var name = getter.Key;

            var getterF = getter.Value;

            Assert.AreEqual("MaybeInts", name);
            var val = getterF(this);
            Assert.IsInstanceOfType(val, typeof(List<int?>));
        }

        [TestMethod]
        public void TestArraysCanTurnedToLists()
        {
            this.StringArray = new[] { "asdf", "qwer", "zxcv" };

            var getter = DataReflector.BuildGetter(this.GetType().GetProperty("StringArray"));

            var name = getter.Key;

            var getterF = getter.Value;

            Assert.AreEqual("StringArray", name);

            Assert.IsInstanceOfType(getterF(this), typeof(List<string>));
        }

        [TestMethod]
        public void TestEnumeralsCanTurnedToLists()
        {
            this.StringE = (IEnumerable<string>) new[] { "asdf", "qwer", "zxcv" }.ToList().Select(x => x);

            var getter = DataReflector.BuildGetter(this.GetType().GetProperty("StringE"));

            var name = getter.Key;

            var getterF = getter.Value;

            Assert.AreEqual("StringE", name);

            Assert.IsInstanceOfType(getterF(this), typeof(List<string>));
        }


        [TestMethod]
        public void TestNullableIntGetterHasValue()
        {
            var pi = this.GetType().GetProperty("NullableInt");
            var expected = 5;
            var getter = DataReflector.BuildGetter(pi);
            this.NullableInt = expected;
            var val = getter.Value(this);

            Assert.AreEqual("NI", getter.Key, "The lookup should be the Scribe Property Name, not the .NET name.");
            Assert.AreEqual(5, val);
        }

        [TestMethod]
        public void TestNullableIntGetterNull()
        {
            var pi = this.GetType().GetProperty("NullableInt");
            int? expected = null;
            var getter = DataReflector.BuildGetter(pi);
            this.NullableInt = expected;
            var val = getter.Value(this);

            Assert.AreEqual("NI", getter.Key, "The lookup should be the Scribe Property Name, not the .NET name.");
            Assert.AreEqual(expected, val);
        }

        [TestMethod]
        public void TestNullableIntType()
        {
            var pi = this.GetType().GetProperty("NullableInt");
            int? expected = 5;
            var getter = DataReflector.BuildGetter(pi);
            this.NullableInt = expected;
            var val = getter.Value(this);

            Assert.AreEqual("NI", getter.Key, "The lookup should be the Scribe Property Name, not the .NET name.");
            Assert.AreEqual(expected, val);
            Assert.IsInstanceOfType(val, typeof(int));
        }

        [TestMethod]
        public void TestStringType()
        {
            var pi = this.GetType().GetProperty("Str");
            this.Str = "MyString";
            var getter = DataReflector.BuildGetter(pi);
            var val = getter.Value(this);

            Assert.AreEqual("Str", getter.Key, "The lookup should be the Scribe Property Name, not the .NET name.");
            Assert.AreEqual(this.Str, val);
            Assert.IsInstanceOfType(val, typeof(string));
        }
    }
}
