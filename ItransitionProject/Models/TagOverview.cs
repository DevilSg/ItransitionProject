using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class TagOverview
    {
        public TagOverview()
        {
            OverTags = new List<OverTag>();
        }

        public int Idtag { get; set; }
        public string TagName { get; set; } = null!;

        public List<OverTag> OverTags { get; set; }
    }
}
