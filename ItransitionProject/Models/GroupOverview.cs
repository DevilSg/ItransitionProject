using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class GroupOverview
    {
        public GroupOverview()
        {
            Overviews = new HashSet<Overview>();
        }

        public int Idgroup { get; set; }
        public string GroupName { get; set; } = null!;

        public virtual ICollection<Overview> Overviews { get; set; }
    }
}
