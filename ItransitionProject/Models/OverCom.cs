using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class OverCom
    {
        public int IdoverCom { get; set; }
        public int Fkcomment { get; set; }
        public int Fkoverviews { get; set; }
        public int Fkusers { get; set; }

        public CommentOverview FkcommentNavigation { get; set; }
        public Overview FkoverviewsNavigation { get; set; }
        public User FkusersNavigation { get; set; }
    }
}
