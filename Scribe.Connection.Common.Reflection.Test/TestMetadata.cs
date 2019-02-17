using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetadataComparer;
using Scribe.Core.ConnectorApi.Metadata;

namespace Scribe.Connection.Common.Reflection.Test
{
    using System.Diagnostics;

    using Scribe.Core.ConnectorApi;

    [TestClass]
    public class TestMetadata
    {
        [TestMethod]
        public void SaveMetadataForTest()
        {
            IConnector conn = null; // Initialize and connect
            var m = MetadataTester.GetMetadata(conn);
            MetadataTester.SaveMetadata(m, "MetadataCompare.json");
        }

        [TestMethod]
        public void CompareMetadata()
        {
            IConnector conn = null; // Initialize and connect
            var mNew = MetadataTester.GetMetadata(conn);

            var mOld = MetadataTester.OpenMetadata("MetadataCompare.json");

            var results = new FullResults(mOld, mNew);
            if (!results.AreSame()) Debug.Print(results.Print());
            Assert.IsTrue(results.AreSame());
        }

        [TestMethod]
        public void CompareFakeMetadataAreSame()
        {
            var od = new ObjectDefinition()
            {
                FullName = "XXX",
                SupportedActionFullNames = new[] {"ASDF"}.ToList()
            };

            var m = new Metadata(new List<IActionDefinition>(), new List<IObjectDefinition>{od});
            var m2 = new Metadata(new List<IActionDefinition>(), new List<IObjectDefinition> { od });

            var results = new FullResults(m, m2);
            Assert.IsTrue(results.AreSame());
        }

        [TestMethod]
        public void CompareFakeMetadataAreDifferent()
        {
            var od = new ObjectDefinition()
            {
                FullName = "XXX",
                SupportedActionFullNames = new[] { "ASDF" }.ToList()
            };
            var od2 = new ObjectDefinition()
            {
                FullName = "YYY",
                SupportedActionFullNames = new[] { "ASDF" }.ToList()
            }; var m = new Metadata(new List<IActionDefinition>(), new List<IObjectDefinition> { od });
            var m2 = new Metadata(new List<IActionDefinition>(), new List<IObjectDefinition> { od, od2 });

            var results = new FullResults(m, m2);
            Assert.IsFalse(results.AreSame());
        }


    }
}
