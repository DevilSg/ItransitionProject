using System;
using System.Collections.Generic;

namespace ItransitionProject.Models
{
    public partial class RoleUser
    {
        public RoleUser()
        {
            Users = new List<User>();
        }

        public int Idrole { get; set; }
        public string RoleName { get; set; } = null!;

        public List<User> Users { get; set; }
    }
}
