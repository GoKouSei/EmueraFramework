using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibrary;
using Framework;

namespace FrameworkTest
{
    [TestClass]
    public class FrameworkTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            TestPlatform platform = new TestPlatform();
            Main framework = new Main();
            platform.Initialize(null, framework);
            framework.Run();
        }
        [TestMethod]
        public void DataBaseTest()
        {
            Main framework = Init();
            framework.Data.ABL[5] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL[5]);
            framework.Data.ABL["HP"] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL["HP"]);
            framework.Data.BASE[1] = 5;
            Assert.AreEqual<long>(5, framework.Data.BASE[1]);

            framework.AddCustomVariable("Custom", 100L);
            Assert.AreEqual<long>(framework.Data.Custom, 100);
            framework.DeleteCustomVariable("Custom");

            Assert.AreEqual(FrameworkState.Initializing, framework.State);
            framework.Run();
            Assert.AreEqual(FrameworkState.Running, framework.State);
            framework.End();
        }

        private static Main Init()
        {
            TestPlatform platform = new TestPlatform();
            Main framework = new Main();
            platform.Initialize(null, framework);
            return framework;
        }

        [TestMethod]
        public void DefaultVariableTest()
        {
            var framework = Init();
            framework.AddChara(0);
            Assert.AreEqual<long>(100, framework.GetChara(0).Data.ABL["HP"]);
        }


    }
    public class TestPlatform : IPlatform
    {
        private IFramework _framework;
        private Tuple<string, Func<object[], object>>[] _methods;
        public Tuple<string, Func<object[], object>>[] methods => _methods;

        public string Name => "TestPlatform";
        
        [SystemFunction(SystemFunctionCode.TITLE)]
        public static object TITLE(object[] args)
        {
            return null;
        }

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            _framework = framework;
            _methods = new Tuple<string, Func<object[], object>>[]
            {
                Tuple.Create<string,Func<object[],object>>("SYSTEM_TITLE", TITLE),
            };
            framework.Initialize
                (new[] { this },
                null,
                new Tuple<string, Type, int>[2] { Tuple.Create("ABL", typeof(long), 1000), Tuple.Create("BASE", typeof(long), 1000) },
                new Tuple<string, Type, int>[1] { Tuple.Create("ABL", typeof(long), 1000) },
                new DefaultCharaInfo[]
                {
                    new DefaultCharaInfo(0, new Dictionary<string, Tuple<object,object>[]>()
                        {
                            {"ABL", new[] { Tuple.Create<object,object>("HP", 100L) } }
                        })
                },
                new Dictionary<string, Dictionary<string, int>>() { { "ABL", new Dictionary<string, int>() { { "HP", 0 } } }, });
        }
    }
}
