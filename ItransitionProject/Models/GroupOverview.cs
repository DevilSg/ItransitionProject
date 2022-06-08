using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class GroupOverview
    {
        public GroupOverview()
        {
            Overviews = new List<Overview>();
        }

        public int Idgroup { get; set; }
        public string GroupName { get; set; } = null!;

        public List<Overview> Overviews { get; set; }
    }
}
