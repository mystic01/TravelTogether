using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Goheer.EXIF;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            const string folderName1 = @"Folder1";
            const string folderName2 = @"Folder2";

            var target = new Executor();
            target.FolderComponents = new List<FolderComponent>
            {
                new FolderComponent(folderName1)
                {
                    TimeStamp = new DateTime(2017, 12, 12, 12, 12, 12, 122),
                    TimeShifting = 100,
                    Author = "Mystic"
                },
                new FolderComponent(folderName2)
                {
                    TimeStamp = new DateTime(2017, 12, 12, 12, 12, 12, 122),
                    TimeShifting = -100,
                    Author = "Nancy"
                }
            };
            CreateAFileInFolder(folderName1);
            CreateAFileInFolder(folderName2);

            target.Rename();

            var pathString1 = Path.Combine(folderName1, "2017_12_12_12_12_12_222_Mystic.jpg");
            var pathString2 = Path.Combine(folderName2, "2017_12_12_12_12_12_022_Nancy.jpg");
            if (!File.Exists(pathString1) || !File.Exists(pathString2))
                Assert.Fail();
        }

        [TestMethod()]
        public void RotateTest_Rotate_Jpg_In_2_Folders_Via_Exif()
        {
            const string folderName1 = @"Folder1";
            const string folderName2 = @"Folder2";

            var target = new Executor();
            target.FolderComponents = new List<FolderComponent>
            {
                new FolderComponent(folderName1),
                new FolderComponent(folderName2)
            };
            CreateAFolder(folderName1);
            CreateAFolder(folderName2);

            var stubJpgName = "LEGO.jpg";
            var stubJpgPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\" + stubJpgName));
            var stubJpgPath1 = Path.Combine(folderName1, stubJpgName);
            var stubJpgPath2 = Path.Combine(folderName2, stubJpgName);
            File.Copy(stubJpgPath, stubJpgPath1, true);
            File.Copy(stubJpgPath, stubJpgPath2, true);
            SetOrientationValue(stubJpgPath1, Orientation.Horizontal);
            SetOrientationValue(stubJpgPath2, Orientation.Rotate90CW);

            target.Rotate();

            var img = Image.FromFile(stubJpgPath);
            var img1 = Image.FromFile(stubJpgPath1);
            var img2 = Image.FromFile(stubJpgPath2);

            if (!(img.Width == img1.Width && img.Height == img1.Height
                && img.Width == img2.Height && img.Height == img2.Width))
                Assert.Fail();
        }

        private void SetOrientationValue(string bitmapPath, Orientation direction)
        {
            const int ExifOrientationId = 0x0112;
            const string tmpFile = "TMPFILE";
            var bmp = new Bitmap(bitmapPath, true);
            if (!bmp.PropertyIdList.Contains(ExifOrientationId))
                return;
            var prop = bmp.GetPropertyItem(ExifOrientationId);
            prop.Value = BitConverter.GetBytes((short)direction);
            bmp.SetPropertyItem(prop);
            bmp.Save(tmpFile, ImageFormat.Jpeg);
            bmp.Dispose();
            File.Delete(bitmapPath);
            File.Move(tmpFile, bitmapPath);
        }

        private string CreateAFileInFolder(string folderName)
        {
            CreateAFolder(folderName);

            var pathString = Path.Combine(folderName, Path.GetRandomFileName() + ".jpg");
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

        private void CreateAFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
            _readyToCleanUpFolder.Add(folderName);
        }
    }
}