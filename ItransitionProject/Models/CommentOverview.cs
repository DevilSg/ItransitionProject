using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class CommentOverview
    {
        public CommentOverview()
        {
            OverComs = new List<OverCom>();
        }

        public int IdcommentOverview { get; set; }
        public string? Comment { get; set; }

        public DateTime DateComment = DateTime.Now;

        public List<OverCom> OverComs { get; set; }
    }
}
