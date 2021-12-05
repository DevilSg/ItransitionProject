using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string ImageUrl { get; set; } = null!;
        public int RateOverview { get; set; }
        public int? LikeUsers { get; set; }
        public int? RateUsers { get; set; }
        public DateTime DateOverview= DateTime.Now;

        [Display(Name = "Image File")]
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public string? StorageName { get; set; }

        public virtual GroupOverview? FkgroupNavigation { get; set; }
        public virtual User? FkuserNavigation { get; set; }
        public virtual ICollection<OverTag> OverTags { get; set; }
    }
}
