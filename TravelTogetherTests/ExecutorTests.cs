using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelTogether;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TravelTogether.Tests
{
    [TestClass()]
    public class ExecutorTests
    {
        private static List<string> _readyToCleanUpFolder = new List<string>();

        [ClassCleanup]
        public static void CleanUp()
        {
            foreach (var folder in _readyToCleanUpFolder)
            {
                foreach (var file in Directory.GetFiles(folder))
                {
                    File.Delete(file);
                }
                Directory.Delete(folder);
            }
        }

        [TestMethod()]
        public void RenameTest_Set_Jpg_In_2_Folders_To_TimeStamp_And_Name()
        {
            var target = new Executor();

            const string folderName1 = @"Folder1";
            var folder1 = new FolderComponent(folderName1)
            {
                TimeStamp = new DateTime(2017, 12, 12, 12, 12, 12, 122),
                TimeShifting = 100,
                Author = "Mystic"
            };
            const string folderName2 = @"Folder2";
            var folder2 = new FolderComponent(folderName2)
            {
                TimeStamp = new DateTime(2017, 12, 12, 12, 12, 12, 122),
                TimeShifting = -100,
                Author = "Nancy"
            };
            target.FolderComponents = new List<FolderComponent>() {folder1, folder2};
            CreateAFileInFolder(folderName1);
            CreateAFileInFolder(folderName2);
            _readyToCleanUpFolder.Add(folderName1);
            _readyToCleanUpFolder.Add(folderName2);

            target.Rename();

            var pathString1 = Path.Combine(folderName1, "2017_12_12_12_12_12_222_Mystic.jpg");
            var pathString2 = Path.Combine(folderName2, "2017_12_12_12_12_12_022_Nancy.jpg");
            if (!File.Exists(pathString1) || !File.Exists(pathString2))
                Assert.Fail();
        }

        private string CreateAFileInFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var pathString = Path.Combine(folderName, Path.GetRandomFileName()+".jpg");
            if (!File.Exists(pathString))
            {
                using (var stream = File.Create(pathString))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        stream.WriteByte(i);
                    }
                }
            }
            return pathString;
        }
    }
}