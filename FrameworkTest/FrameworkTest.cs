using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibrary;
using SharedLibrary.Data;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrameworkTest
{
    [TestClass]
    public class FrameworkTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            Main framework = Init();
            framework.Run();
        }
        [TestMethod]
        public void DataBaseTest()
        {
            Main framework = Init();
            framework.Data.ABL = 100;
            Assert.AreEqual(100, (long)framework.Data.ABL);
            framework.Data.ABL[5] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL[5]);
            framework.Data["ABL", 10] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL[10]);
            framework.Data.ABL["HP"] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL["HP"]);
            framework.Data["ABL", "HP"] = 10;
            Assert.AreEqual<long>(10, framework.Data.ABL["HP"]);
            framework.Data["BASE"] = 5;
            Assert.AreEqual<long>(5, framework.Data.BASE[0]);

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

        [TestMethod]
        public void FunctionCallTest()
        {
            var framework = Init();
            framework.Run();
            framework.End();
            Assert.AreEqual<long>(9999, framework.Data.BASE[100]);
        }


    }
    public class TestPlatform : IPlatform
    {
        private IFramework _framework;

        public Method[] methods => null;

        public string Name => "TestPlatform";

        public SystemFunction[] systemFunctions
        {
            get
            {
                return new SystemFunction[] 
                {
                    new SystemFunction(SystemFunctionCode.TITLE, TITLE),
                };
            }
        }

        public static void TITLE(IFramework framework)
        {
            framework.Data.BASE[100] = 9999;
        }

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            _framework = framework;

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
                new NameDictionary() { { "ABL", new Dictionary<string, int>() { { "HP", 0 } } }, });
        }
    }
}
