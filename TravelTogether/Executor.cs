using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    }
}