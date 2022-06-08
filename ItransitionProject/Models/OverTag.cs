using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class OverTag
    {
        public int IdoverTag { get; set; }
        public int Fktag { get; set; }
        public int Fkoverview { get; set; }

        public Overview FkoverviewNavigation { get; set; }
        public TagOverview FktagNavigation { get; set; }
    }
}
