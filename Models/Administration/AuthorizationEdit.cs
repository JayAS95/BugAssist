using BugAssist.Models.Administration;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BugAssist.Models.Administration
{
    public class AuthorizationEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}