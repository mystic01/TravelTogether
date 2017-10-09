using System.Collections.Generic;
using System.Windows.Documents;
using FluentAssert;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TravelTogether.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void AddFolderTest_AddASimplePath()
        {
            var target = new MainWindow();
            var expected = @"C:\";

            target.AddFolder(@"C:\");

            var actual = target.TogetherExecutor.FolderComponents[0].Path;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DeleteFolderTest_DeleteAPath()
        {
            var target = new MainWindow();
            target.AddFolder(@"C:\");
            target.AddFolder(@"D:\");
            var expectedSize = 1;
            var expectedPath = @"C:\";

            target.DeleteFolder(1);

            Assert.AreEqual(expectedSize, target.TogetherExecutor.FolderComponents.Count);
            Assert.AreEqual(expectedPath, target.TogetherExecutor.FolderComponents[0].Path);
        }
    }
}