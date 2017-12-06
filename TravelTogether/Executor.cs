using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using Goheer.EXIF;

namespace TravelTogether
{
    internal class Executor
    {
        public List<FolderComponent> FolderComponents { get; internal set; }

        public Executor()
        {
            FolderComponents = new List<FolderComponent>();
        }

        public void Rename()
        {
            foreach (var folderCompo in FolderComponents)
            {
                var folder = folderCompo.Path;
                foreach (var file in Directory.GetFiles(folder, "*.jpg"))
                {
                    var timeFormat = @"yyyy_MM_dd_HH_mm_ss_fff";
                    var newFileName = Path.Combine(folder,
                        folderCompo.TimeStamp.AddMilliseconds(folderCompo.TimeShifting).ToString(timeFormat)
                        + "_" + folderCompo.Author + ".jpg");
                    File.Move(file, newFileName);
                }
            }
        }

        public void Rotate()
        {
            const string tmpFile = "TMPFILE";
            foreach (var folderCompo in FolderComponents)
            {
                var folder = folderCompo.Path;
                foreach (var file in Directory.GetFiles(folder, "*.jpg"))
                {
                    var rotateFilpType = GetRotateFilpTypeViaExif(file);
                    var bmp = new Bitmap(file);
                    bmp.RotateFlip(rotateFilpType);
                    bmp.Save(tmpFile, ImageFormat.Jpeg);
                    bmp.Dispose();
                    Thread.Sleep(2000);
                    File.Delete(file);
                    File.Move(tmpFile, file);
                }
            }
        }

        private RotateFlipType GetRotateFilpTypeViaExif(string file)
        {
            var bmp = new Bitmap(file);
            var exif = new EXIFextractor(ref bmp, "\n");
            int orientationId = int.Parse((exif["Orientation"] ?? "0") as string);
            bmp.Dispose();

            switch (orientationId)
            {
                case (int) Orientation.Horizontal:
                    return RotateFlipType.RotateNoneFlipNone;
                case (int) Orientation.MirrorHorizontal:
                    return RotateFlipType.RotateNoneFlipX;
                case (int) Orientation.Rotate180:
                    return RotateFlipType.Rotate180FlipNone;
                case (int) Orientation.MirrorVertical:
                    return RotateFlipType.Rotate180FlipX;
                case (int) Orientation.MirrorHorizontalAndRotate270CW:
                    return RotateFlipType.Rotate90FlipX;
                case (int) Orientation.Rotate90CW:
                    return RotateFlipType.Rotate90FlipNone;
                case (int) Orientation.MirrorHorizontalAndRotate90CW:
                    return RotateFlipType.Rotate270FlipX;
                case (int) Orientation.Rotate270CW:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }
}