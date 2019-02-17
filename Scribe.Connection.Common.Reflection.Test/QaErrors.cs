using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scribe.Connector.Common.Reflection;
using Scribe.Connector.Common.Reflection.Data;
using Scribe.Connector.Common.Reflection.PropertyType;

namespace Scribe.Connection.Common.Reflection.Test
{
    [TestClass]
    public class QaErrors
    {
        [TestMethod]
        public void NullStringShouldBeNullValueInDataEntity()
        {
            var hs = new HasString { MyStr = null };


            var refl = new Reflector(typeof(HasString));

            var de = refl.ToDataEntity(hs);
            var actual = de.Properties["MyStr"];
            Assert.AreNotEqual(String.Empty, actual, "Null string in the model should NOT be an empty string in DataEntity.");
            Assert.IsNull(actual);

        }

        [TestMethod]
        public void NullStringShouldBeNullonverter()
        {
            object o = null;

            var actual = SimpleTypeConverters.ConvertToString(o);

            Assert.AreNotEqual(String.Empty, actual, "Null object converted to string should NOT be an empty string.");
            Assert.IsNull(actual);
        }
    }

    [ObjectDefinition]
    public class HasString
    {
        [PropertyDefinition]
        public string MyStr { get; set; }
    }
}