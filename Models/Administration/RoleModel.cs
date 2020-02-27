using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BugAssist.Models.Administration
{
    public class RoleModel : IdentityRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
