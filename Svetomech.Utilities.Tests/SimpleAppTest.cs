using NUnit.Framework;
using System;
using System.IO;
using static Svetomech.Utilities.SimpleApp;

namespace Svetomech.Utilities.Tests
{
    [TestFixture]
    public class SimpleAppTest
    {
        [Test]
        public void IsElevated_False()
        {
            Assert.IsFalse(IsElevated());
        }

        [Test, Order(1)]
        public void VerifyAutorun_SomeApp_False()
        {
            string appName = "SomeApp";
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.IsFalse(isAppAutorun);
        }

        [Test, Order(2)]
        public void SwitchAutorun_SomeApp_Add()
        {
            string appName = "SomeApp";
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

        [Test, Order(3)]
        public void VerifyAutorun_SomeApp_True()
        {
            string appName = "SomeApp";
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.IsTrue(isAppAutorun);
        }

        [Test, Order(4)]
        public void SwitchAutorun_SomeApp_Remove()
        {
            string appName = "SomeApp";

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