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

        public FolderComponent(string path)
        {
            Path = path;
        }
    }
}
