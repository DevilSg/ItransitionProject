using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class RoleUser
    {
        public RoleUser()
        {
            Users = new HashSet<User>();
        }

        public int Idrole { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
