﻿using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class Overview
    {
        public Overview()
        {
            OverTags = new HashSet<OverTag>();
        }

        public int Idoverview { get; set; }
        public string Title { get; set; } = null!;
        public int? Fkgroup { get; set; }
        public int? Fkuser { get; set; }
        public string TextOverview { get; set; } = null!;
        public string PictureOverview { get; set; } = null!;
        public int RateOverview { get; set; }
        public int? LikeUsers { get; set; }
        public int? RateUsers { get; set; }

        public DateTime DateOverview = DateTime.Now;

        public virtual GroupOverview? FkgroupNavigation { get; set; }
        public virtual User? FkuserNavigation { get; set; }
        public virtual ICollection<OverTag> OverTags { get; set; }
       
    }
}
