using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelTogether
{
    internal class Executor
    {
        public List<FolderComponent> FolderComponents { get; set; }

        public Executor()
        {
            FolderComponents = new List<FolderComponent>();
        }
    }
}
