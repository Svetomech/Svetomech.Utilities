using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Svetomech.Utilities.SimpleApp;

namespace Svetomech.Utilities.Tests
{
    [TestClass]
    public class SimpleAppTest
    {
        [TestMethod]
        public void IsElevated_False()
        {
            Assert.IsFalse(IsElevated());
        }

        [TestMethod]
        public void VerifyAutorun_SomeApp_False()
        {
            string appName = "SomeApp";
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.IsFalse(isAppAutorun);
        }

        [TestMethod]
        public void SwitchAutorun_OtherApp_Add()
        {
            string appName = "OtherApp";
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            try
            {
                SwitchAutorun(appName, appPath);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void VerifyAutorun_OtherApp_True()
        {
            string appName = "OtherApp";
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.IsTrue(isAppAutorun);
        }

        [TestMethod]
        public void SwitchAutorun_OtherApp_Remove()
        {
            string appName = "OtherApp";

            try
            {
                SwitchAutorun(appName);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
