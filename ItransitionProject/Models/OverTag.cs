using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class OverTag
    {
        public int IdoverTag { get; set; }
        public int Fktag { get; set; }
        public int Fkoverview { get; set; }

        public virtual Overview FkoverviewNavigation { get; set; } = null!;
        public virtual TagOverview FktagNavigation { get; set; } = null!;
    }
}
