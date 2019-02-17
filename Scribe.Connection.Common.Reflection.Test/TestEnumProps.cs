using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionTestModels;
using Scribe.Connector.Common.Reflection;
using Scribe.Connector.Common.Reflection.Data;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Actions;

namespace Scribe.Connection.Common.Reflection.Test {

    [TestClass]
    public class TestCoreBugProps
    {
        [TestMethod]
        public void TestBadOperationInput()
        {
            var child = new CoreBugWorkaround {Str = "Child"};
            var testData = new CoreBugWorkaround{Str = "Root", PropName = child};

            var dataRefl = new Reflector(typeof(CoreBugWorkaround));
            var de = dataRefl.ToDataEntity(testData);

            // Make the BUG (set the objDef name to the PropName)
            de.Children["PropName"][0].ObjectDefinitionFullName = "PropName";

            var result = dataRefl.To<CoreBugWorkaround>(de);
            Assert.IsTrue(true);
        }
    }


    [TestClass]
    public class TestEnumProps
    {
        public static IMetadata m = MetadataReflector.ReflectMetadata(typeof(HasEnums));
        public static IMetadataProvider mp = m.Wrap();

        [TestMethod]
        public void TestEnumPropertyMetadata()
        {
            //var x = new HasEnums().Address;
            var od = m.Types["HasEnums"];
            var prop = od.PropertyDefinitions.Single(pd => pd.FullName == "Address");

            Assert.AreEqual("System.String", prop.PropertyType);
            Assert.AreEqual("This property is based on the enum type Options. Defined values are None(0), Shipping(1), Billing(2), Home(3).", prop.Description);
        }

        [TestMethod]
        public void TestEnumPropertyMetadataWithAdvancedDescription()
        {
            //var x = new HasEnums().Address;
            var od = m.Types["HasEnums"];
            var prop = od.PropertyDefinitions.Single(pd => pd.FullName == "WithDescription");

            Assert.AreEqual("System.String", prop.PropertyType);
            Assert.AreEqual("This is a precursor description." + " This property is based on the enum type Options. Defined values are None(0), Shipping(1), Billing(2), Home(3).", prop.Description);
        }

        [TestMethod]
        public void TestEnumToDEProperty()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);

            Assert.AreEqual("Home", de.Properties["Address"]);
        }

        [TestMethod]
        public void TestEnumPropertyRoundTrip()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);

            var newE = r.To<HasEnums>(de);
            Assert.AreEqual(e.Address, newE.Address);
        }

        [TestMethod]
        public void TestEnumPropertySetByInt()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);
            de.Properties["Address"] = 1;
            var newE = r.To<HasEnums>(de);
            Assert.AreEqual(Options.Shipping, newE.Address);
        }

        [TestMethod]
        public void TestEnumPropertySetByString()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);
            de.Properties["Address"] = "Shipping";
            var newE = r.To<HasEnums>(de);
            Assert.AreEqual(Options.Shipping, newE.Address);
        }

        [TestMethod]
        public void TestEnumPropertySetByIntAsString()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);
            de.Properties["Address"] = "1";
            var newE = r.To<HasEnums>(de);
            Assert.AreEqual(Options.Shipping, newE.Address);
        }

        [TestMethod]
        public void FailTestEnumPropertySetByBadString()
        {
            var r = new Reflector(typeof(HasEnums));
            var e = new HasEnums();
            e.Address = Options.Home;

            var de = r.ToDataEntity(e);
            de.Properties["Address"] = "NotValid";
            try { var newE = r.To<HasEnums>(de); }
            catch (InvalidStringValueForEnumException ex)
            {
                Assert.AreEqual("The value 'NotValid' is not a valid entry for enum type 'ReflectionTestModels.Options'. Valid values are None, Shipping, Billing, Home.", ex.Message);
                return;
            }
            Assert.Fail("Expected InvalidStringValueForEnumException was not thrown.");
        }


        // Need unit tests for
        private Reflector weird = new Reflector(typeof(HasWeirdEnums));

        // Empty Enumerations (Should be Empty String) Setting by Empty String, Null == Null (Not sure what that means)
        [TestMethod]
        public void TestEmptyEnumSetEmpty() // Advice here should be -- do not do this.
        {
            var e = new HasWeirdEnums();
            e.NoEnum = (Unit) 5;

            var de = this.weird.ToDataEntity(e);
            Assert.AreEqual("5", de.Properties["NoEnum"]);
        }


        // Setting with the Wrong Strings (Good error message)

        // Setting with Wrong Ints (Good error message)

        // Strange indexes 4,7,9,etc. Should be reflected in Description and setting

        // Strange underlying types: byte, int, uint, etc. (Should work)
    }
}