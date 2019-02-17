namespace Scribe.Connection.Common.Reflection.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scribe.Connector.Common.Reflection;
    using Scribe.Connector.Common.Reflection.PropertyType;
    using Scribe.Connector.Common.Reflection.PropertyType.Specialized;

    [TestClass]
    public class TestSimpleSettersGetters
    {
        public static IFullProp GetPropertyDef(PropertyInfo pi)
        {
            var propDef = pi.GetCustomAttribute<PropertyDefinitionAttribute>();

            var name = pi.Name;
            var ty = pi.PropertyType;
            Func<object, object> getter = pi.GetValue;
            Action<object, object> setter = pi.SetValue;

            if (pi.PropertyType == typeof(string)) return new StringProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(int)) return new Int32Property(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(int?)) return new NInt32Property(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(List<string>)) return new StringListProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(IEnumerable<string>)) return new StringListProperty(propDef, name, ty, getter, setter);

            if (pi.PropertyType == typeof(List<int>)) return new Int32ListProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(IEnumerable<int>)) return new Int32ListProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(int[])) return new Int32ListProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(int?[])) return new NInt32ListProperty(propDef, name, ty, getter, setter);
            if (pi.PropertyType == typeof(List<int?>)) return new NInt32ListProperty(propDef, name, ty, getter, setter);

            throw new InvalidOperationException("The Test GetPropertyDef implementation for Tests has not been configured for this type yet: " + pi.PropertyType.FullName);
        }

        [TestMethod]
        public void TestSimpleStringProp()
        {
            var props = typeof(Q).GetProperties().Select(GetPropertyDef);

            var q = new Q();

            props.Where(x => x is IProp && x.Name == "Str").Cast<IProp>().First().Set(q, "asdf");

            Assert.AreEqual("asdf", q.Str);
        }

        [TestMethod]
        public void TestSimpleIntProp()
        {
            var props = typeof(Q).GetProperties().Select(GetPropertyDef);

            var q = new Q();

            props.First(pd => pd.Name == "Int").Set(q, 7);

            Assert.AreEqual(7, q.Int);
        }

        [TestMethod]
        public void TestNullableIntPropToInt()
        {
            var props = typeof(Q).GetProperties().Select(GetPropertyDef);

            var q = new Q();

            props.First(pd => pd.Name == "NInt").Set(q, 7);

            Assert.AreEqual(7, q.NInt);
        }

        [TestMethod]
        public void TestNullableIntPropToNull()
        {
            var props = typeof(Q).GetProperties().Select(GetPropertyDef);

            var q = new Q();

            props.First(pd => pd.Name == "NInt").Set(q, null);

            Assert.IsNull(q.NInt);
        }

        [TestMethod]
        public void TestSimpleListSet()
        {
            var props = typeof(WithLists).GetProperties().Select(GetPropertyDef);

            var w = new WithLists();

            props.First(pd => pd.Name == "Strings").Set(w, new List<string> { "asdf", "qwer", null });

            Assert.AreEqual(3, w.Strings.Count);
        }

        [TestMethod]
        public void TestSimpleListGet()
        {
            var props = typeof(WithLists).GetProperties().Select(GetPropertyDef);

            var w = new WithLists();

            w.Strings = new List<string> { "asdf", "qwer", null };

            var prop = props.First(pd => pd.Name == "Strings") as IProp<List<string>>;

            var actual = prop.Get(w);

            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void TestSimpleIEnumerableSet()
        {
            var props = typeof(WithLists).GetProperties().Select(GetPropertyDef);

            var w = new WithLists();

            props.First(pd => pd.Name == "StringEs").Set(w, new List<string> { "asdf", "qwer", null }.AsEnumerable());

            Assert.AreEqual(3, w.StringEs.Count());
        }

        [TestMethod]
        public void TestSimpleIEnumerableGet()
        {
            var props = typeof(WithLists).GetProperties().Select(GetPropertyDef);

            var w = new WithLists();

            w.StringEs = new List<string> { "asdf", "qwer", null };

            var prop = props.First(pd => pd.Name == "StringEs") as IProp<List<string>>;

            var actual = prop.Get(w);

            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void TestSimpleArraySet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            props.First(pd => pd.Name == "IntArray").Set(w, new[] { 1, 2, 3 }.AsEnumerable());

            Assert.AreEqual(3, w.IntArray.Count());
        }

        [TestMethod]
        public void TestSimpleArrayGet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            w.IntArray = new []{ 1, 2, 3 };

            var prop = props.First(pd => pd.Name == "IntArray") as IProp<List<int>>;

            var actual = prop.Get(w);

            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void TestSimpleNListSet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            props.First(pd => pd.Name == "NInts").Set(w, new int?[] { 1, 2, 3, null }.AsEnumerable());

            Assert.AreEqual(4, w.NInts.Count());
        }

        [TestMethod]
        public void TestSimpleNListGet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            w.NInts = new List<int?> { 1, 2, 3, null };

            var prop = props.First(pd => pd.Name == "NInts") as IProp<List<int?>>;

            var actual = prop.Get(w);

            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        public void TestSimpleNArraySet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            props.First(pd => pd.Name == "NIntArray").Set(w, new int?[] { 1, 2, 3, null }.AsEnumerable());

            Assert.AreEqual(4, w.NIntArray.Count());
        }

        [TestMethod]
        public void TestSimpleNArrayGet()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            w.NIntArray = new int?[] { 1, 2, 3, null };

            var prop = props.First(pd => pd.Name == "NIntArray") as IProp<List<int?>>;

            var actual = prop.Get(w);

            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        public void TestSimpleNArrayGetNullProp()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();

            w.NIntArray = null;

            var prop = props.First(pd => pd.Name == "NIntArray") as IProp<List<int?>>;

            var actual = prop.Get(w);

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestSimpleNArraySetNullProp()
        {
            var props = typeof(WithLists).GetProperties()
                .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
                .Select(GetPropertyDef);

            var w = new WithLists();


            var prop = props.First(pd => pd.Name == "NIntArray") as IProp<List<int?>>;

            prop.Set(w, null);

            Assert.AreEqual(null, w.NIntArray);
        }

        //[TestMethod]
        //[ExpectedException(ExceptionType = typeof(InvalidOperationException))]
        //public void TestTargetIsNullException()
        //{
        //    var props = typeof(WithLists).GetProperties()
        //        .Where(pd => pd.GetCustomAttributes<PropertyDefinitionAttribute>().Any())
        //        .Select(GetPropertyDef);

        //    var w = new WithLists();


        //    var prop = props.First(pd => pd.Name == "NIntArray") as IProp<List<int?>>;

        //    prop.Set(null, null);

        //    Assert.AreEqual(null, w.NIntArray);
        //}
    }

    [ObjectDefinition]
    public class WithLists
    {
        [PropertyDefinition]
        public List<string> Strings { get; set; }

        [PropertyDefinition]
        public IEnumerable<string> StringEs { get; set; }

        [PropertyDefinition]
        public List<int> Ints { get; set; }

        [PropertyDefinition]
        public List<int?> NInts { get; set; }

        [PropertyDefinition]
        public int?[] NIntArray { get; set; }

        [PropertyDefinition]
        public int[] IntArray { get; set; }

    }
}
