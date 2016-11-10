using NUnit.Framework;
using System;
using System.IO;
using static Svetomech.Utilities.App;

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
            VerifyAutorun_False("SomeApp");
        }

        [Test, Order(2)]
        public void SwitchAutorun_SomeApp_Add()
        {
            SwitchAutorun_Add("SomeApp");
        }

        [Test, Order(3)]
        public void VerifyAutorun_SomeApp_True()
        {
            VerifyAutorun_True("SomeApp");
        }

        [Test, Order(4)]
        public void SwitchAutorun_SomeApp_Remove()
        {
            SwitchAutorun_Remove("SomeApp");
        }

        [Test, Order(5)]
        public void VerifyAutorun_OtherApp_False()
        {
            VerifyAutorun_False("OtherApp");
        }

        [Test, Order(6)]
        public void SwitchAutorun_OtherApp_Add()
        {
            SwitchAutorun_Add("OtherApp", true);
        }

        [Test, Order(7)]
        public void VerifyAutorun_OtherApp_True()
        {
            VerifyAutorun_True("OtherApp");
        }

        [Test, Order(8)]
        public void SwitchAutorun_OtherApp_Remove()
        {
            SwitchAutorun_Remove("OtherApp");
        }

        public void VerifyAutorun_False(string appName)
        {
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.False(isAppAutorun);
        }

        public void SwitchAutorun_Add(string appName, bool isConsoleApp = false)
        {
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            try
            {
                SwitchAutorun(appName, appPath, isConsoleApp);
            }
            catch
            {
                Assert.Fail();
            }
        }

        public void VerifyAutorun_True(string appName)
        {
            string appPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName, $"{appName}.exe");

            bool isAppAutorun = VerifyAutorun(appName, appPath);

            Assert.True(isAppAutorun);
        }

        public void SwitchAutorun_Remove(string appName)
        {
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