using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreMvcPractice.Data.Models
{
    public class UserRole : IdentityRole<int>
    {
        public UserRole() { }

        public UserRole(string roleName) : base(roleName) { }
    }
}
