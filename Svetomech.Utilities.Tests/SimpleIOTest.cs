using System;
using NUnit.Framework;
using System.IO;
using System.Threading;

namespace Svetomech.Utilities.Tests
{
    [TestFixture]
    public class SimpleIOTest
    {
        [Test, Order(1)]
        public void Lock_SomeFile_NoAccess()
        {
            string someFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SomeFile");
            var someFile = new FileInfo(someFilePath);

            someFile.Create();
            someFile.Lock();

            // Give file creation some time
            Thread.Sleep(1000);

            Assert.IsTrue(someFile.IsLocked());
            /*try
            {
                using (var reader = someFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)) ;
            }
            catch
            {
                Assert.Pass();
            }

            Assert.Fail();*/
        }

        /*[Test, Order(2)]
        public void Unlock_SomeFile_Access()
        {
            string someFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SomeFile");
            var someFile = new FileInfo(someFilePath);

            someFile.Unlock();

            Assert.IsFalse(someFile.IsLocked());

            someFile.Delete();
        }*/
    }
}
