using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class User
    {
        public User()
        {
            Overviews = new HashSet<Overview>();
        }

        public int Iduser { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public int? Fkrole { get; set; }

        public virtual RoleUser? FkroleNavigation { get; set; }
        public virtual ICollection<Overview> Overviews { get; set; }
    }
}
