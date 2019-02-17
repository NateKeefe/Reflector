using System.Diagnostics;

namespace Scribe.Connection.Common.Reflection.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.CSharp.RuntimeBinder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestGenericLists
    {
        [TestMethod]
        public void TestGettingSingleTypeParam()
        {
            
        }

        [TestMethod]
        public void TestGettingGenericList()
        {

        }

        [TestMethod]
        public void TestGettingNonGenericList()
        {

        }

        [TestMethod]
        public void TestDetectingEnumerable()
        {
            var tyParam = GetUnderlyingType(this.GetPropType("IntList"));

            Assert.AreEqual(typeof(int), tyParam);
        }

        private static Type GetUnderlyingType(Type t)
        {
            return t.GenericTypeArguments[0];
        }

        private Type GetPropType(string name)
        {
            return this.GetType().GetRuntimeProperty(name).PropertyType;
        }

        public IEnumerable<int> Ints { get; set; }
        public List<int> IntList { get; set; }

        [TestMethod]
        public void TestGettingGenericListDynamic()
        {
            this.Test = new[] { "asdf", "asdf" };

            var propInfo = this.GetType().GetProperty("Test");
            Debug.Assert(propInfo != null);
            var exceptionThrown = false;
            try
            {
                dynamic raw = propInfo.GetValue(this);

                var unused = raw.ToList();
            }
            catch (RuntimeBinderException)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }


        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IEnumerable<string> Test { get; set; }
        public SortedList ObjList { get; set; }
    }
}