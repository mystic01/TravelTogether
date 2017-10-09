using System.Collections.Generic;
using System.Windows.Documents;
using ExpectedObjects;
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
            var expected = new List<FolderComponent> { new FolderComponent(@"C:\") };

            target.AddFolder(@"C:\");

            var actual = target.TogetherExecutor.FolderComponents;
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod()]
        public void DeleteFolderTest_DeleteAPath()
        {
            var target = new MainWindow();
            target.AddFolder(@"C:\");
            target.AddFolder(@"D:\");
            var expected = new List<FolderComponent> { new FolderComponent(@"C:\") };

            target.DeleteFolder(1);

            var actual = target.TogetherExecutor.FolderComponents;
            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}