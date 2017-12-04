using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelTogether
{
    internal class FolderComponent
    {
        public string Path { get; private set; }
        public int TimeShifting { get; set; }
        public string Author { get; set; }
        public DateTime TimeStamp { get; set; }

        public FolderComponent(string path)
        {
            Path = path;
        }
    }
}
