using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class TagOverview
    {
        public TagOverview()
        {
            OverTags = new HashSet<OverTag>();
        }

        public int Idtag { get; set; }
        public string TagName { get; set; } = null!;

        public virtual ICollection<OverTag> OverTags { get; set; }
    }
}
