using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scribe.Connection.Common.Reflection.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Connector.Common.Reflection;
    using Scribe.Connector.Common.Reflection.PropertyType;

    [TestClass]
    public class TestHierarchicalProperties
    {
        [TestMethod]
        public void TestRecursivePropertyDef()
        {
            var x = MetadataReflector.BuildObjectDefHeader(typeof(Recurser));

            Assert.AreEqual(1, x.Properties.Values.Count());
        }

        [TestMethod]
        public void TestToDataEntity()
        {

            var ox = new ObjX { Abc = "asdf" };

            var o = MetadataReflector.BuildObjectDefHeader(typeof(ObjX));

            var enricher = new ObjDefs(new[] { o });
            var dataEntity = ObjDefConverter.To(ox, enricher);
            var objx = ObjDefConverter.From<ObjX>(dataEntity, enricher);
            
            Assert.AreEqual(1, dataEntity.Properties.Count);
            Assert.AreEqual("ObjX", dataEntity.ObjectDefinitionFullName);
            Assert.AreEqual("asdf", objx.Abc);
            Assert.AreNotSame(ox, objx);
        }

        //public static ObjDef SecondPass(ObjDefHeader h)
        //{
        //    var a = h.Actions.Values.Select(ad => ad);
        //    var ps = h.Properties.Values.Select(x => Enrich(x, h));
        //    var obj = new ObjDef(h.Name, h.Description, h.Hidden, a, ps);
        //    return obj;
        //}

        //public static IPropDef Enrich(IPropDef pd, ObjDefHeader h)
        //{
        //    if (!pd.IsObjectDefProp)
        //    {
        //        return pd;
        //    }

        //    var depd = pd as DataEntityPropertyDef;
        //    var dep = new DataEntityProperty<Recurser2>(depd, h);
        //    return dep;
        //}


        [TestMethod]
        public void TestRecursieveGettingAndSetting()
        {

            var oxchild = new Recurser2 { S = "asdf" };
            var oxRoot = new Recurser2 { S = "qwer", R = oxchild };
            var o = MetadataReflector.BuildObjectDefHeader(typeof(Recurser2));

            var enricher = new ObjDefs(new[] { o });
            

            var dataEntity = ObjDefConverter.To(oxRoot, enricher);
            var objx = ObjDefConverter.From<Recurser2>(dataEntity, enricher);

            Assert.AreEqual(1, dataEntity.Properties.Count);
            Assert.AreEqual("Recurser2", dataEntity.ObjectDefinitionFullName);
            Assert.AreEqual("qwer", objx.S);
            Assert.AreNotSame(oxRoot, objx);

            Assert.AreEqual("asdf", objx.R.S);
        }

        [TestMethod]
        public void TestMutuallyRecursieveGettingAndSetting()
        {

            var o1 = MetadataReflector.BuildObjectDefHeader(typeof(A));
            var o2 = MetadataReflector.BuildObjectDefHeader(typeof(B));

            var enricher = new ObjDefs(new[] { o1, o2 });


            var dataEntity = ObjDefConverter.To(
                    Helper.MakeA(), enricher);

            var a = ObjDefConverter.From<A>(dataEntity, enricher);

            Assert.AreEqual(1, dataEntity.Properties.Count);
            Assert.AreEqual("A", dataEntity.ObjectDefinitionFullName);
            Assert.AreEqual("XYZ", a.S);
            Assert.AreEqual("ABC", a.B.A.S);

        }
        [TestMethod]
        public void TestComplexLists()
        {

            var o1 = MetadataReflector.BuildObjectDefHeader(typeof(A));
            var o2 = MetadataReflector.BuildObjectDefHeader(typeof(HasLists));
            var ob = MetadataReflector.BuildObjectDefHeader(typeof(B));

            var enricher = new ObjDefs(new[] { o1, o2, ob });

            var hl = new HasLists { As = new List<A> { Helper.MakeA() } };

            var dataEntity = ObjDefConverter.To(hl, enricher);

            var a = ObjDefConverter.From<HasLists>(dataEntity, enricher);

            Assert.AreEqual(0, dataEntity.Properties.Count);
            Assert.AreEqual("HasLists", dataEntity.ObjectDefinitionFullName);
            Assert.AreEqual("XYZ", a.As.First().S);
            Assert.AreEqual("ABC", a.As.First().B.A.S);
        }

        [TestMethod]
        public void TestGettingNullLists()
        {

            var o1 = MetadataReflector.BuildObjectDefHeader(typeof(A));
            var o2 = MetadataReflector.BuildObjectDefHeader(typeof(HasLists));
            var ob = MetadataReflector.BuildObjectDefHeader(typeof(B));

            var enricher = new ObjDefs(new[] { o1, o2, ob });

            var hl = new HasLists { As = null };

            var dataEntity = ObjDefConverter.To(
                hl, enricher);

            var a = ObjDefConverter.From<HasLists>(dataEntity, enricher);

            Assert.AreEqual(0, dataEntity.Properties.Count);
            Assert.AreEqual("HasLists", dataEntity.ObjectDefinitionFullName);
            Assert.IsTrue(a.As.Count == 0);
        }
    }



    [ObjectDefinition]
    public class Recurser
    {
        [PropertyDefinition]
        public Recurser R { get; set; }
    }
    [ObjectDefinition]
    public class Recurser2
    {
        [PropertyDefinition]
        public string S { get; set; }
        [PropertyDefinition]
        public Recurser2 R { get; set; }
    }

    [ObjectDefinition]
    public class A
    {
        [PropertyDefinition] public B B { get; set; }

        [PropertyDefinition] 
        public string S { get; set; }
    }

    public static class Helper 
    {
        public static A MakeA()
        {
            return new A { B = new B { A = new A { S = "ABC" } }, S = "XYZ" };
        }
    }

    [ObjectDefinition]
    public class B
    {
        [PropertyDefinition]
        public A A { get; set; }
    }

    [ObjectDefinition]
    public class ObjX
    {
        [PropertyDefinition]
        public string Abc { get; set; }
    }

[ObjectDefinition]
    public class HasLists
    {
        [PropertyDefinition]
        public List<A> As { get; set; }
    }
}
