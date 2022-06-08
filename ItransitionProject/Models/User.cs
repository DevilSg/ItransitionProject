using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class User
    {
        public User()
        {
            OverComs = new List<OverCom>();
            Overviews = new List<Overview>();
        }

        public int Iduser { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public int? Fkrole { get; set; }

        public RoleUser? FkroleNavigation { get; set; }
        public List<OverCom> OverComs { get; set; }
        public List<Overview> Overviews { get; set; }
    }
}
